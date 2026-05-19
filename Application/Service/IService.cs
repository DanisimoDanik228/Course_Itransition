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
        IEnumerable<PartNameCustomId> GetAllPartCustomId();
        Task<IEnumerable<Item>> GetAllItemsFromInventoryAsync (long idInventory);
        Task<IEnumerable<InventoryResponseDto>> GetAllInventoryAsync();
        Task<IEnumerable<InventoryResponseDto>> GetAllInventoryUserAsync(string userId);
        Task<IEnumerable<InventoryResponseDto>> GetAccessInventoryUserAsync(string userId);
        Task<string> GetStructCustomIdAsync(long inventoryId);
        Task<InventoryFullResponseDto?> GetFullInventoryByIdAsync(long Id);
        Task<InventoryFullResponseDto?> GetPartInventoryAsync(long Id, int Count, int Page);   
        Task<InventoryResponseDto?> AddInventoryAsync(InventoryRequestDto item);
        Task<ItemFullResponseDto?> AddItemAsync(ItemFullRequestDto item);
        Task AddItemValueAsync(long inventoryId, AddItemValueRequestDto[] itemValue);
        Task<InventoryTypeResponseDto?> AddFieldAsync(InventoryTypeRequestDto item);
        Task<int?> DeleteInventoryAsync(long[] idInventory);
        Task<int?> DeleteItemsAsync(long idInventory, long[] itemsId);
        Task<int?> DeleteFieldAsync(long idInventory, long[] fieldsId);
        Task SetStructCustomIdAsync(long idInventory, string structCustomId);
        Task UpdateItemAsync(UpdateItemRequestDto[] data, long idInventory);
    }
}
 