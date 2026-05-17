using Application.Dto.Response;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Repository.User
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
        Task<bool> LoginAsync(string email, string password);
        Task LogoutAsync();
        Task<bool> RegisterAsync(string email, string password);
        Task BlockUserAsync(string[] Ids);
        Task UnblockUserAsync(string[] Ids);
        Task MakeAdminAsync(string[] userId);
        Task RemoveAdminAsync(string[] userId);
        Task DeleteUsersAsync(string[] Ids);
    }
}
