using Application.Dto.Response;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Repository.User
{
    public interface IEditorRepository
    {
        Task<bool?> MakeEditorAsync(string[] userId, long inventoryId);
        Task<bool?> RemoveEditorAsync(string[] userId, long inventoryId);
        Task<List<InventoryEditorResponseDto>> GetEditorInventoryAsync(long inventoryId);
    }
}
