using Application.Dto.Request;
using Application.Dto.Response;
using Application.Repository.User;
using Application.Service;
using Domain.Models;
using Infrastructure.Repository.PostgresDbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Xml.Linq;

namespace Infrastructure.Service.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEditorRepository _editorRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly IEditorSearchService _editorSearchService;

        public UserService(
            IUserRepository userRepository,
            IEditorRepository editorRepository,
            IAuthenticationService authenticationService,
            IEditorSearchService editorSearchService
            )
        {
            _userRepository = userRepository;
            _editorRepository = editorRepository;
            _authenticationService = authenticationService;
            _editorSearchService = editorSearchService;
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }
        public async Task<List<InventoryEditorResponseDto>> GetEditorInventoryAsync(long inventoryId)
        {
            return await _editorRepository.GetEditorInventoryAsync(inventoryId);
        }
        public async Task<List<InventoryEditorResponseDto>> FindEditorInventoryAsync(long inventoryId, string userName, string searchField)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return await _editorRepository.GetEditorInventoryAsync(inventoryId);
            }

            if (searchField == "Email")
            {
                return await _editorSearchService.FindEditorInventoryByEmailAsync(inventoryId, userName);
            }
            else 
            { 
                return await _editorSearchService.FindEditorInventoryByNameAsync(inventoryId, userName);
            }
        }
        public async Task<bool> LoginAsync(string email, string password)
        {
            if (email == null || email.Length < 4)
            {
                return false;
            }
            if (email == null || email.Length < 4)
            {
                return false;
            }

            return await _userRepository.LoginAsync(email, password);
        }

        public async Task LogoutAsync()
        {
            await _userRepository.LogoutAsync();
        }

        public async Task<bool> RegisterAsync(string name, string email, string password)
        {
            if (name == null || name.Length < 4)
            {
                return false;
            }
            if (email == null || email.Length < 4)
            {
                return false;
            }
            if (email == null || email.Length < 4)
            {
                return false;
            }

            var user = await _userRepository.RegisterAsync(name, email, password);
            
            if (user != null)
            { 
                await _editorSearchService.IndexUserAsync(user);
            }

            return user != null;
        }

        public async Task BlockUserAsync(string[] Ids)
        {
            await _userRepository.BlockUserAsync(Ids);
        }

        public async Task UnblockUserAsync(string[] Ids)
        {
            await _userRepository.UnblockUserAsync(Ids);
        }
        public async Task DeleteUsersAsync(string[] Ids)
        {
            await _editorSearchService.DeleteUserAsync(Ids);

            await _userRepository.DeleteUsersAsync(Ids);
        }

        public async Task MakeAdminRoleAsync(string[] userId)
        {
            await _userRepository.MakeAdminAsync(userId);
        }

        public async Task RemoveAdminRoleAsync(string[] userId)
        {
            await _userRepository.RemoveAdminAsync(userId);
        }

        public async Task<bool?> MakeEditorRoleAsync(string[] userId, long inventoryId)
        {
            var myId = _authenticationService.MyId();
            if (!(await _authenticationService.MayDropAndCreateEditor(myId, inventoryId)))
            {
                return null;
            }

            return await _editorRepository.MakeEditorAsync(userId ,inventoryId);
        }

        public async Task<bool?> RemoveEditorRoleAsync(string[] userId, long inventoryId)
        {
            var myId = _authenticationService.MyId();
            if (!(await _authenticationService.MayDropAndCreateEditor(myId, inventoryId)))
            {
                return null;
            }

            return await _editorRepository.RemoveEditorAsync(userId, inventoryId);
        }
    }
}
