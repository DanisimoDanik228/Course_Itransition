using Application;
using Application.Repository.Tables;
using Domain.Models;
using Infrastructure.Repository.PostgresDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository.Repository.Tables
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

        public async Task<IEnumerable<Inventory>> GetAccessInventoryUserAsync(string userId)
        {
            var user = await _context.Users
                .Include(u => u.EditInventory)
                .ThenInclude(ei => ei.Inventory)
                .Include(u => u.CreatedInventory)
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            var accessInventories = user.CreatedInventory.Union(user.EditInventory.Select(ei => ei.Inventory));
            return accessInventories;
        }

        public async Task<IEnumerable<Inventory>> GetAllAsync()
        {
            return _context.Inventory.AsNoTracking().AsEnumerable();
        }

        public async Task<IEnumerable<Inventory>> GetAllInventoryUserAsync(string userId)
        {
            var inventories = await _context.Users
                .AsNoTracking()
                .Include(u => u.CreatedInventory)
                .FirstOrDefaultAsync(u => u.Id == userId);
            
            return inventories.CreatedInventory;
        }

        public Task<Inventory?> GetFullByIdAsync(long Id)
        {
            return _context.Inventory
                .Include(i => i.Creator)
                .Include(i => i.InventoryType)
                .Include(i => i.Items)
                .ThenInclude(i => i.ItemValue)
                .AsNoTracking()
                .FirstAsync(i => i.Id == Id);
        }

        public async Task<IEnumerable<InventoryType>> GetInventoryTypeOnInventoryAsync(long inventoryId)
        {
            var inventory = await _context.Inventory
                .Include(i => i.InventoryType)
                .Where(i => i.Id == inventoryId)
                .FirstOrDefaultAsync();

            return inventory.InventoryType;
        }

        public async Task<long> GetMaxSequenceAsync(long inventoryId)
        {
            if (await _context.Items.CountAsync() == 0)
            {
                return 0;
            }

            return _context.Items
                .Where(i => i.InventoryId == inventoryId)
                .Max(i => i.Sequence);
        }

        public async Task<int[]> GetOrderField(long inventoryId)
        {
            var inventory = await _context.Inventory
                .FindAsync(inventoryId);

            return inventory.OrderField;
        }

        public async Task<Inventory?> GetPartByIdAsync(long Id, int page, int countItem)
        {
            var inventory = await _context.Inventory
                .Include(i => i.Creator)
                .Include(i => i.InventoryType)
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == Id);

            inventory.Items = await _context.Items
                .Where(i => i.InventoryId == Id)
                .OrderBy(i => i.Id)
                .Skip((page - 1) * countItem)
                .Take(countItem)
                .Include(i => i.ItemValue)
                .ThenInclude(i => i.InventoryType)
                .AsNoTracking()
                .ToListAsync();

            return inventory;
        }

        public async Task<string> GetStructCustomIdAsync(long inventoryId)
        {
            var inventory = await _context.Inventory.FindAsync(inventoryId);
            return inventory.StructCustomId;
        }

        public async Task<Inventory?> UpdateAsync(Inventory item)
        {
            var res = _context.Inventory.Update(item);
            return res.Entity;
        }

        public async Task UpdateCustomIdAsync(long inventoryId, string structCustomId)
        {
            await _context.Inventory
                .Where(i => i.Id == inventoryId)
                .ExecuteUpdateAsync(i => i.SetProperty(i => i.StructCustomId, structCustomId));
        }

        public async Task UpdateOrderFieldAsync(long inventoryId, int[] orderField)
        {
            await _context.Inventory
                .Where(i => i.Id == inventoryId)
                .ExecuteUpdateAsync(i => i.SetProperty(inv => inv.OrderField, orderField));
        }
    }
}
