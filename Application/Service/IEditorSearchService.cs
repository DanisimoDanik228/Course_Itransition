using Application.Dto.Response;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Service
{
    public interface IEditorSearchService
    {
        Task<List<InventoryEditorResponseDto>> FindEditorInventoryByNameAsync(long inventoryId, string userName);
        Task<List<InventoryEditorResponseDto>> FindEditorInventoryByEmailAsync(long inventoryId, string email);
        Task IndexUserAsync(AppUser user);
        Task DeleteUserAsync(string[] userIds);
    }
}
