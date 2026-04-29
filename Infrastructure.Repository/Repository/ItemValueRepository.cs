using Application.Repository;
using Domain.Models;
using Infrastructure.Repository.PostgresDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Repository
{
    public class ItemValueRepository : IItemValueRepository
    {
        private readonly AppDbContext _context;

        public ItemValueRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ItemValue?> AddAsync(ItemValue item)
        {
            var res = await _context.ItemValue.AddAsync(item);
            await _context.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<ItemValue?> DeleteAsync(ItemValue item)
        {
            var res = _context.ItemValue.Remove(item);
            await _context.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<IEnumerable<ItemValue>> GetAllAsync()
        {
            return _context.ItemValue.AsNoTracking().AsEnumerable();
        }

        public async Task<ItemValue?> UpdateAsync(ItemValue item)
        {
            var res = _context.ItemValue.Update(item);
            await _context.SaveChangesAsync();
            return res.Entity;
        }
    }
}