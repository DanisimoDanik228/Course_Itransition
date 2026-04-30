using Application.Dto.Response;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Service
{
    public interface IService
    {
        Task<IEnumerable<Item>> GetAllItemsFromInventoryAsync(long idInventory);
        Task<IEnumerable<Inventory>> GetAllInventoryAsync();
        Task<Inventory?> GetFullInventoryByIdAsync(long Id);
        Task<InventoryResponseDto?> AddInventoryAsync(Inventory item);
    }
}
