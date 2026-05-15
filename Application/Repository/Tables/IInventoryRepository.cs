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
        Task<Inventory?> AddAsync(Inventory item);
        Task<Inventory?> UpdateAsync(Inventory item);
        Task<Inventory?> DeleteAsync(Inventory item);
        Task<IEnumerable<Inventory>> GetAllInventoryUserAsync(string userId);
        Task<int> DeleteAsync(long[] Ids);
    }
}
