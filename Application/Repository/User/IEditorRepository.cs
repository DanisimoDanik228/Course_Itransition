using Application.Dto.Response;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Repository.User
{
    public interface IEditorRepository
    {
        Task MakeEditorAsync(string[] userId, long inventoryId);
        Task RemoveEditorAsync(string[] userId, long inventoryId);
        Task<List<InventoryEditorResponseDto>> GetEditorInventoryAsync(long inventoryId);
    }
}
