using Application.Repository;
using Domain.Models;
using Infrastructure.Repository.PostgresDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository.Repository
{
    public class InventoryTypeRepository : IInventoryTypeRepository
    {
        private readonly AppDbContext _context;

        public InventoryTypeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<InventoryType?> AddAsync(InventoryType item)
        {
            var res = await _context.InventoryType.AddAsync(item);
            await _context.SaveChangesAsync();

            return res.Entity;
        }

        public async Task<InventoryType?> DeleteAsync(InventoryType item)
        {
            var res = _context.InventoryType.Remove(item);
            await _context.SaveChangesAsync();
            return res.Entity;
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
    }
}
