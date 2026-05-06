using Application.Dto.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Service
{
    public interface IAccountService
    {
        Task<bool> Register(string email, string password);
        Task<bool> Login(string email, string password);
        Task Logout();
        Task<IEnumerable<UserRequest>> GetAllUsers();
        Task DeleteUsers(string[] Ids);
    }
}
