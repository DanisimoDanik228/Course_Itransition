using Application.Dto.Response;
using Application.Repository.User;
using Domain.Models;
using Infrastructure.Repository.PostgresDbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository.Repository.User
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _context;

        public UserRepository(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task DeleteUsersAsync(string[] Ids)
        {
            await _userManager.Users.Where(u => Ids.Contains(u.Id)).ExecuteDeleteAsync();
        }

        public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
        {
            var userRoles =
                await _context.Users
                    .Select(user => new UserResponse
                    {
                        Id = user.Id,
                        Email = user.Email,
                        IsBlocked = user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTimeOffset.UtcNow,
                        Role = _context.UserRoles
                            .Where(ur => ur.UserId == user.Id)
                            .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                            .ToList()
                    })
                    .ToListAsync();

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
            var user = new AppUser { UserName = email, Email = email };
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
            var roles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user,roles);

            await _userManager.AddToRoleAsync(user, status);
            await _signInManager.RefreshSignInAsync(user);
        }
    }
}
