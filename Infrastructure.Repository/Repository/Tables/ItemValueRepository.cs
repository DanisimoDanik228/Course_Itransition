using Application.Dto.Request;
using Application.Repository.Tables;
using Domain.Models;
using Infrastructure.Repository.PostgresDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Repository.Tables
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

        public async Task AddRangeAsync(ItemValue[] item)
        {
            await _context.ItemValue.AddRangeAsync(item);
            await _context.SaveChangesAsync();
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

        public async Task UpdateAsync(List<UpdateItemRequestDto> data)
        {
            for (int i = 0; i < data.Count; i++)
            {
                var id = data[i].ItemValueId;
                var value = data[i].Value;

                await _context.ItemValue
                    .Where(v => v.Id == id)
                    .ExecuteUpdateAsync(s => s.SetProperty(v => v.Value, value));
            }
        }

        public async Task<bool> ExistCustomIdAsync(long inventoryTypeCustomId, string customIds)
        {
            var res = await _context.ItemValue
                .Where(it => it.InventoryTypeId == inventoryTypeCustomId)
                .Select(it => it.Value)
                .AnyAsync(v => v == customIds);

            return res;
        }
    }
}