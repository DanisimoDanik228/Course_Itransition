using Application;
using Application.Repository;
using Domain.Models;
using Infrastructure.Repository.PostgresDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository.Repository
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly AppDbContext _context;

        public InventoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Inventory?> AddAsync(Inventory item)
        {
            var res =  await _context.Inventory.AddAsync(item);
            await _context.SaveChangesAsync();

            return res.Entity;
        }

        public async Task<Inventory?> DeleteAsync(Inventory item)
        {
            var res = _context.Inventory.Remove(item);
            await _context.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<int> DeleteAsync(long[] Ids)
        {
            return await _context.Inventory.Where(i => Ids.Contains(i.Id)).ExecuteDeleteAsync();
        }

        public async Task<IEnumerable<Inventory>> GetAllAsync()
        {
            return _context.Inventory.AsNoTracking().AsEnumerable();
        }

        public Task<Inventory?> GetFullByIdAsync(long Id)
        {
            return _context.Inventory
                .Include(i => i.InventoryType)
                .Include(i => i.Items)
                .ThenInclude(i => i.ItemValue)
                .AsNoTracking()
                .FirstAsync(i => i.Id == Id);
        }

        public async Task<Inventory?> GetPartByIdAsync(long Id, int page, int countItem)
        {
            var inventory = await _context.Inventory
                .Include(i => i.InventoryType)
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == Id);

            inventory.Items = await _context.Items
                .Where(i => i.InventoryId == Id)
                .OrderBy(i => i.Id)
                .Skip((page - 1) * countItem)
                .Take(countItem)
                .Include(i => i.ItemValue)
                .AsNoTracking()
                .ToListAsync();

            return inventory;
        }

        public async Task<Inventory?> UpdateAsync(Inventory item)
        {
           var res = _context.Inventory.Update(item);
            return res.Entity;
        }
    }
}
