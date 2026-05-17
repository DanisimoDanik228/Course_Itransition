using Application.Dto.Request;
using Application.Dto.Request.Full;
using Application.Dto.Response;
using Application.Dto.Response.Full;
using Application.Repository.Tables;
using Application.Service;
using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Service.Service
{
    public class Service : IService
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IInventoryTypeRepository _inventoryTypeRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;
        public Service(
            IInventoryRepository inventoryRepository,
            IItemRepository itemRepository,
            IInventoryTypeRepository inventoryTypeRepository,
            IAuthenticationService authenticationService,
            IMapper mapper )
        {
            _inventoryRepository = inventoryRepository;
            _itemRepository = itemRepository;
            _inventoryTypeRepository = inventoryTypeRepository;
            _authenticationService = authenticationService;
            _mapper = mapper;
        }
        public async Task<IEnumerable<InventoryResponseDto>> GetAllInventoryAsync()
        {
            return (await _inventoryRepository.GetAllAsync()).Select(i => _mapper.Map<Inventory,InventoryResponseDto>(i));
        }

        public async Task<IEnumerable<Item>> GetAllItemsFromInventoryAsync(long idInventory)
        {
            return await _itemRepository.GetAllFromInventoryAsync(idInventory);
        }

        public async Task<InventoryFullResponseDto?> GetFullInventoryByIdAsync(long Id)
        {
            var res = await _inventoryRepository.GetFullByIdAsync(Id);
            var response = _mapper.Map<Inventory, InventoryFullResponseDto>(res);

            return PrepareFullInventoryToShow(response);
        }

        public async Task<InventoryFullResponseDto?> GetPartInventoryAsync(long Id, int Count, int Page)
        {
            var res = await _inventoryRepository.GetPartByIdAsync(Id, Page, Count);
            var response = _mapper.Map<Inventory, InventoryFullResponseDto>(res); 

            return PrepareFullInventoryToShow(response);
        }

        public async Task<IEnumerable<InventoryResponseDto>> GetAllInventoryUserAsync(string userId)
        {
            var inventories = await _inventoryRepository.GetAllInventoryUserAsync(userId);

            return inventories.Select(i => _mapper.Map<Inventory, InventoryResponseDto>(i));
        }
        public async Task<InventoryResponseDto?> AddInventoryAsync(InventoryRequestDto item)
        {
            var inventory = _mapper.Map<InventoryRequestDto, Inventory>(item);
            var res = await _inventoryRepository.AddAsync(inventory);

            return _mapper.Map<Inventory, InventoryResponseDto>(res);
        }

        public async Task<ItemFullResponseDto?> AddItemAsync(ItemFullRequestDto item)
        {
            var itemFull = _mapper.Map<ItemFullRequestDto, Item>(item);
            var res = await _itemRepository.AddAsync(itemFull);

            return _mapper.Map<Item, ItemFullResponseDto>(res);
        }

        public async Task<InventoryTypeResponseDto?> AddFieldAsync(InventoryTypeRequestDto item)
        {
            var inventoryType = _mapper.Map<InventoryTypeRequestDto, InventoryType>(item);
            var res = await _inventoryTypeRepository.AddAsync(inventoryType);

            return _mapper.Map<InventoryType, InventoryTypeResponseDto>(res);
        }

        private static InventoryFullResponseDto PrepareFullInventoryToShow(InventoryFullResponseDto inventory)
        {
            var nullItemValue = new ItemValueResponseDto();
            nullItemValue.Name = "Null_name";
            nullItemValue.Value = "Null";

            inventory.InventoryType.Sort((a, b) => string.Compare(a.Name, b.Name));

            for (int i = 0; i < inventory.Items.Count; i++)
            {
                var item = inventory.Items[i];
                item.ItemValue.Sort((a, b) => string.Compare(a.Name, b.Name));

                int indexInventoryType = 0;
                var newListItenValue = new List<ItemValueResponseDto>();

                for (int j = 0; j < item.ItemValue.Count(); j++)
                {
                    var item1 = item.ItemValue[j];

                    while (indexInventoryType < inventory.InventoryType.Count() &&
                        0 < string.Compare(item1.Name, inventory.InventoryType[indexInventoryType].Name))
                    {

                        newListItenValue.Add(nullItemValue);
                        indexInventoryType++;
                    }

                    if (indexInventoryType < inventory.InventoryType.Count() &&
                        item1.Name == inventory.InventoryType[indexInventoryType].Name)
                    {
                        newListItenValue.Add(item1);
                        indexInventoryType++;
                    }
                }

                while (indexInventoryType < inventory.InventoryType.Count())
                {

                    newListItenValue.Add(nullItemValue);
                    indexInventoryType++;
                }

                item.ItemValue = newListItenValue;
            }

            return inventory;
        }

        public async Task<int> DeleteInventoryAsync(long[] idInventory)
        {
            return await _inventoryRepository.DeleteAsync(idInventory);
        }

        public async Task<int> DeleteItemsAsync(long idInventory, long[] itemsId)
        {
            return await _itemRepository.DeleteAsync(itemsId);
        }

        public async Task<int> DeleteFieldAsync(long idInventory, long[] fieldsId)
        {
            return await _inventoryTypeRepository.DeleteAsync(fieldsId);
        }
    }
}