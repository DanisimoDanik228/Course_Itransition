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
        Task<IEnumerable<Item>> GetAllItemsFromInventoryAsync(long idInventory);
        Task<IEnumerable<InventoryResponseDto>> GetAllInventoryAsync();
        Task<int> DeleteInventoryAsync(long[] idInventory);
        Task<InventoryFullResponseDto?> GetFullInventoryByIdAsync(long Id);
        Task<InventoryFullResponseDto?> GetPartInventoryAsync(long Id, int Count, int Page);
        Task<int> DeleteItemsAsync(long idInventory, long[] itemsId);
        Task<int> DeleteFieldAsync(long idInventory, long[] fieldsId);
        Task<InventoryResponseDto?> AddInventoryAsync(InventoryRequestDto item);
        Task<ItemFullResponseDto?> AddItemAsync(ItemFullRequestDto item);
        Task<InventoryTypeResponseDto?> AddFieldAsync(InventoryTypeRequestDto item);
        Task<IEnumerable<InventoryResponseDto>> GetAllInventoryUserAsync(string userId);
    }
}
 