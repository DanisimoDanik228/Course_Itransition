using Application.Dto.Request;
using Application.Service;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Service.Service
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task DeleteUsersAsync(string[] Ids)
        {
            foreach (var item in Ids)
            {
                var user = await _userManager.FindByIdAsync(item);
                await _userManager.DeleteAsync(user);
            }
        }

        public async Task<IEnumerable<UserRequest>> GetAllUsersAsync()
        {
            var users = _userManager.Users.ToList();
            var userRoles = new List<UserRequest>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var responseUser = new UserRequest { 
                    Id = user.Id, 
                    Email = user.Email, 
                    Role = roles,
                    IsBlocked = user.LockoutEnd > DateTimeOffset.UtcNow
                };

                userRoles.Add(responseUser);
            }

            return userRoles;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: false);

            return result.Succeeded;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> RegisterAsync(string email, string password)
        {
            var user = new IdentityUser { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Registered");
            }

            return result.Succeeded;
        }

        public async Task BlockUserAsync(string[] Ids)
        {
            var user = await _userManager.FindByIdAsync(Ids[0]);

            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(123));
            await _userManager.UpdateSecurityStampAsync(user);
        }

        public async Task UnblockUserAsync(string[] Ids)
        {
            var user = await _userManager.FindByIdAsync(Ids[0]);

            await _userManager.SetLockoutEndDateAsync(user, null);
            await _userManager.UpdateSecurityStampAsync(user);
        }

        public async Task SetUserStatusAsync(string[] userId, string status)
        {
            var user = await _userManager.FindByIdAsync(userId[0]);
            await _userManager.AddToRoleAsync(user, status);
        }
    }
}
