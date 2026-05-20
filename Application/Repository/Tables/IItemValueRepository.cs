using Application.Dto.Request;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Repository.Tables
{
    public interface IItemValueRepository
    {
        Task<IEnumerable<ItemValue>> GetAllAsync();
        Task<ItemValue?> AddAsync(ItemValue item);
        Task AddRangeAsync(ItemValue[] item);
        Task<bool> ExistCustomIdAsync(long inventoryTypeCustomId, string customIds);
        Task<ItemValue?> UpdateAsync(ItemValue item);
        Task UpdateAsync(List<UpdateItemRequestDto> data);
        Task<ItemValue?> DeleteAsync(ItemValue item);
    }
}
