using Application.Dto.Response;
using Application.Repository;
using Application.Service;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Infrastructure.Service.Service
{
    public class Service : IService
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IItemRepository _itemRepository;
        public Service(
            IInventoryRepository inventoryRepository,
            IItemRepository itemRepository)
        {
            _inventoryRepository = inventoryRepository;
            _itemRepository = itemRepository;
        }
        public async Task<InventoryResponseDto?> AddInventoryAsync(Inventory item)
        {
            var res = await _inventoryRepository.AddAsync(item);
            var res1 = new InventoryResponseDto() {Id=res.Id,Name=res.Name};
            return res1;
        }

        public async Task<IEnumerable<Inventory>> GetAllInventoryAsync()
        {
            return await _inventoryRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Item>> GetAllItemsFromInventoryAsync(long idInventory)
        {
            return await _itemRepository.GetAllFromInventoryAsync(idInventory);
        }

        public async Task<Inventory?> GetFullInventoryByIdAsync(long Id)
        {
            return await _inventoryRepository.GetFullByIdAsync(Id);
        }
    }
}
