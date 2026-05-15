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
using System.Text;

namespace Infrastructure.Service.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task DeleteUsersAsync(string[] Ids)
        {
            await _userRepository.DeleteUsersAsync(Ids);
        }

        public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
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

        public async Task SetUserStatusAsync(string[] userId, string status)
        {
            await _userRepository.SetUserStatusAsync(userId, status);
        }
    }
}
