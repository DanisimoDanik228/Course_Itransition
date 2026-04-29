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

        public async Task<IEnumerable<Inventory>> GetAllAsync()
        {
            return _context.Inventory.AsNoTracking().AsEnumerable();
        }

        public async Task<Inventory?> UpdateAsync(Inventory item)
        {
           var res = _context.Inventory.Update(item);
            return res.Entity;
        }
    }
}
