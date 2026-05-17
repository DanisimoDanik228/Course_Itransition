using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Repository.Tables
{
    public interface IInventoryTypeRepository
    {
        Task<IEnumerable<InventoryType>> GetAllAsync();
        Task<InventoryType?> AddAsync(InventoryType item);
        Task<InventoryType?> UpdateAsync(InventoryType item);
        Task<InventoryType?> DeleteAsync(InventoryType item);
        Task<int> DeleteAsync(long[] Ids);
    }
}
