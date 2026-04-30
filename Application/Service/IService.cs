using Application.Dto.Response;
using Application.Dto.Response.Full;
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
        Task<InventoryFullResponseDto?> GetFullInventoryByIdAsync(long Id);
        Task<InventoryResponseDto?> AddInventoryAsync(Inventory item);
    }
}
