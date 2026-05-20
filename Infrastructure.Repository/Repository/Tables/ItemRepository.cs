using Application.Repository.Tables;
using Domain.Models;
using Infrastructure.Repository.PostgresDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Repository.Tables
{
    public class ItemRepository : IItemRepository
    {
        private readonly AppDbContext _context;

        public ItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Item?> AddAsync(Item item)
        {
            var res = await _context.Items.AddAsync(item);
            await _context.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<Item?> DeleteAsync(Item item)
        {
            var res = _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<int> DeleteAsync(long[] Ids)
        {
            return await _context.Items.Where(i => Ids.Contains(i.Id)).ExecuteDeleteAsync();
        }

        public async Task<IEnumerable<Item>> GetAllAsync()
        {
            return _context.Items.AsNoTracking().AsEnumerable();
        }

        public async Task<IEnumerable<Item>> GetAllFromInventoryAsync(long idInventory)
        {
            return _context.Items.Include(i => i.ItemValue).AsNoTracking().Where(i => i.InventoryId == idInventory).ToList();
        }

        public async Task<Item?> GetByIdAsync(long Id)
        {
            return await _context.Items
                .FindAsync(Id);
        }

        public async Task<Item?> UpdateAsync(Item item)
        {
            var res = _context.Items.Update(item);
            await _context.SaveChangesAsync();
            return res.Entity;
        }
    }
}