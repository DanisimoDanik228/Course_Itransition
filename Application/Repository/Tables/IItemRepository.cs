using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Repository.Tables
{
    public interface IItemRepository
    {
        Task<Item?> GetByIdAsync(long Id);
        Task<IEnumerable<Item>> GetAllAsync();
        Task<IEnumerable<Item>> GetAllFromInventoryAsync(long idInventory);
        Task<Item?> AddAsync(Item item);
        Task<Item?> UpdateAsync(Item item);
        Task<Item?> DeleteAsync(Item item);
        Task<int> DeleteAsync(long[] Ids);
    }
}
