using Application.Dto.Request;
using Application.Dto.Request.Full;
using Application.Dto.Response;
using Application.Dto.Response.Full;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Application.Service
{
    public interface IService
    {
        InventoryFullResponseDto PrepareFullInventoryToShow(InventoryFullResponseDto item);
        Task<IEnumerable<Item>> GetAllItemsFromInventoryAsync(long idInventory);
        Task<IEnumerable<Inventory>> GetAllInventoryAsync();
        Task<int> DeleteInventoryAsync(long[] idInventory);
        Task<InventoryFullResponseDto?> GetFullInventoryByIdAsync(long Id);
        Task<InventoryResponseDto?> AddInventoryAsync(Inventory item);
        Task<ItemFullResponseDto?> AddItemAsync(ItemFullRequestDto item);
        Task<InventoryTypeResponseDto?> AddFieldAsync(InventoryTypeRequestDto item);
    }
}
 