using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Repository.Tables
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<Inventory>> GetAllAsync();
        Task<Inventory?> GetFullByIdAsync(long Id);
        Task<Inventory?> GetPartByIdAsync(long Id, int page, int countItem);
        Task<IEnumerable<Inventory>> GetAllInventoryUserAsync(string userId);
        Task<IEnumerable<Inventory>> GetAccessInventoryUserAsync(string userId);
        Task<string> GetStructCustomIdAsync(long inventoryId);
        Task<IEnumerable<InventoryType>> GetInventoryTypeOnInventoryAsync(long inventoryId);
        Task<long> GetMaxSequenceAsync(long inventoryId);
        Task<int[]> GetOrderField(long inventoryId);
        Task<bool> GetStatusAsync(long inventoryId);
        Task<Inventory?> AddAsync(Inventory item);
        Task UpdateCustomIdAsync(long inventoryId, string structCustomId);
        Task UpdateStatusInventoryAsync(long inventoryId, bool status);
        Task<Inventory?> UpdateAsync(Inventory item);
        Task UpdateOrderFieldAsync(long inventoryId, int[] orderField);
        Task<Inventory?> DeleteAsync(Inventory item);
        Task<int> DeleteAsync(long[] Ids);
    }
}
