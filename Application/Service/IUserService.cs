using Application.Dto.Request;
using Application.Dto.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Service
{
    public interface IUserService
    {
        Task<bool> RegisterAsync(string email, string password);
        Task<bool> LoginAsync(string email, string password);
        Task LogoutAsync();
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
        Task DeleteUsersAsync(string[] Ids);
        Task BlockUserAsync(string[] Ids);
        Task UnblockUserAsync(string[] Ids);
        Task MakeAdminRoleAsync(string[] userId);
        Task RemoveAdminRoleAsync(string[] userId);
        Task MakeEditorRoleAsync(string[] userId, string myId, long inventoryId);
        Task RemoveEditorRoleAsync(string[] userId, string myId, long inventoryId);
        Task<List<InventoryEditorResponseDto>> GetEditorInventoryAsync(long inventoryId);
    }
}
