using Application.Dto.Response;
using Application.Dto.Response.Full;
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

        public async Task<InventoryFullResponseDto?> GetFullInventoryByIdAsync(long Id)
        {
            return ParseInventoryToFull(await _inventoryRepository.GetFullByIdAsync(Id));
        }

        public InventoryFullResponseDto ParseInventoryToFull(Inventory inventory)
        {
            return new InventoryFullResponseDto
            {
                Id = inventory.Id,
                Name = inventory.Name,

                Items = inventory.Items?.Select(item => ParseItemToFull(item)).ToList() ?? new(),

                InventoryType = inventory.InventoryType?.Select(it => new IventoryTypeResponseDto
                {
                    Id = it.Id,
                    Type = it.Type,
                    Name = it.Name
                }).ToList() ?? new()
            };
        }
        public static ItemFullResponseDto ParseItemToFull(Item item)
        {
            return new ItemFullResponseDto
            {
                Id = item.Id,
                ItemValue = item.ItemValue?.Select(iv => new ItemValueResponseDto
                {
                    Id = iv.Id,
                    Value = iv.Value,
                    Type = iv.Type,
                    Name = iv.Name
                }).ToList() ?? new()
            };
        }
    }
}