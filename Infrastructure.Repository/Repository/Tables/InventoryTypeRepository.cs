using Application.Options;
using Application.Repository.Tables;
using Domain.Models;
using Infrastructure.Repository.PostgresDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository.Repository.Tables
{
    public class InventoryTypeRepository : IInventoryTypeRepository
    {
        private readonly AppDbContext _context;
        private readonly InventorySettings _inventorySettings;

        public InventoryTypeRepository(
            AppDbContext context,
            IOptions<InventorySettings> options)
        {
            _context = context;
            _inventorySettings = options.Value;
        }

        public async Task<InventoryType?> AddAsync(InventoryType item)
        {
            var res = await _context.InventoryType.AddAsync(item);
            await _context.SaveChangesAsync();

            return res.Entity;
        }
        public async Task<long> GetIdItemValueCustomIdAsync(long inventoryId)
        {
            return await _context.InventoryType
                .Where(it => it.InventoryId == inventoryId && it.Name == "CustomId")
                .Select(it => it.Id)
                .FirstOrDefaultAsync();
        }
        public async Task<int> DeleteAsync(long[] Ids)
        {
            return await _context.InventoryType
                .Where(i => Ids.Contains(i.Id) && i.Name != _inventorySettings.CustomIdName)
                .ExecuteDeleteAsync();
        }

        public async Task<IEnumerable<InventoryType>> GetAllAsync()
        {
            return _context.InventoryType.AsNoTracking().AsEnumerable();
        }

        public async Task<InventoryType?> UpdateAsync(InventoryType item)
        {
            var res = _context.InventoryType.Update(item);
            return res.Entity;
        }

        public async Task UpdateRangeAsync(IEnumerable<InventoryType> data)
        {
            _context.InventoryType.UpdateRange(data);
            await _context.SaveChangesAsync();
        }
    }
}
