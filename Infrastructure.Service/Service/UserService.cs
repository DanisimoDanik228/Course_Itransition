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

namespace Infrastructure.Service.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEditorRepository _editorRepository;

        public UserService(
            IUserRepository userRepository,
            IEditorRepository editorRepository
            )
        {
            _userRepository = userRepository;
            _editorRepository = editorRepository;
        }

        public async Task DeleteUsersAsync(string[] Ids)
        {
            await _userRepository.DeleteUsersAsync(Ids);
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            return await _userRepository.LoginAsync(email, password);
        }

        public async Task LogoutAsync()
        {
            await _userRepository.LogoutAsync();
        }

        public async Task<bool> RegisterAsync(string email, string password)
        {
            return await _userRepository.RegisterAsync(email, password);
        }

        public async Task BlockUserAsync(string[] Ids)
        {
            await _userRepository.BlockUserAsync(Ids);
        }

        public async Task UnblockUserAsync(string[] Ids)
        {
            await _userRepository.UnblockUserAsync(Ids);
        }

        public async Task MakeAdminRoleAsync(string[] userId)
        {
            await _userRepository.MakeAdminAsync(userId);
        }

        public async Task RemoveAdminRoleAsync(string[] userId)
        {
            await _userRepository.RemoveAdminAsync(userId);
        }

        public async Task MakeEditorRoleAsync(string[] userId, string myId, long inventoryId)
        {
            await _editorRepository.MakeEditorAsync(userId ,inventoryId);
        }

        public async Task RemoveEditorRoleAsync(string[] userId, string myId, long inventoryId)
        {
            await _editorRepository.RemoveEditorAsync(userId, inventoryId);
        }

        public async Task<List<InventoryEditorResponseDto>> GetEditorInventoryAsync(long inventoryId)
        {
            return await _editorRepository.GetEditorInventoryAsync(inventoryId);
        }
    }
}
