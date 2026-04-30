using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Repository
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<Inventory>> GetAllAsync();
        Task<Inventory?> GetFullByIdAsync(long Id);
        Task<Inventory?> AddAsync(Inventory item);
        Task<Inventory?> UpdateAsync(Inventory item);
        Task<Inventory?> DeleteAsync(Inventory item);
    }
}
