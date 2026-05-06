using Application.Dto.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Service
{
    public interface IAccountService
    {
        Task<bool> RegisterAsync(string email, string password);
        Task<bool> LoginAsync(string email, string password);
        Task LogoutAsync();
        Task<IEnumerable<UserRequest>> GetAllUsersAsync();
        Task DeleteUsersAsync(string[] Ids);
        Task BlockUserAsync(string[] Ids);
        Task UnblockUserAsync(string[] Ids);
        Task SetUserStatusAsync(string[] userId, string status);
    }
}
