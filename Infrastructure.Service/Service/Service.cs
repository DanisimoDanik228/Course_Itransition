using Application.Dto.Request;
using Application.Dto.Request.Full;
using Application.Dto.Response;
using Application.Dto.Response.Full;
using Application.Options;
using Application.Repository.Tables;
using Application.Service;
using AutoMapper;
using Domain.Models;
using Infrastructure.Repository.Repository.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        private readonly IItemValueRepository _itemValueRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IInventoryTypeRepository _inventoryTypeRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly ICustomIdService _customIdService;
        private readonly InventorySettings _inventorySettings;
        private readonly IMapper _mapper;
        public Service(
            IInventoryRepository inventoryRepository,
            IItemValueRepository itemValueRepository,
            IItemRepository itemRepository,
            IInventoryTypeRepository inventoryTypeRepository,
            IAuthenticationService authenticationService,
            ICustomIdService customIdService,
            IOptions<InventorySettings> options,
            IMapper mapper )
        {
            _inventoryRepository = inventoryRepository;
            _itemValueRepository = itemValueRepository;
            _itemRepository = itemRepository;
            _inventoryTypeRepository = inventoryTypeRepository;
            _authenticationService = authenticationService;
            _customIdService = customIdService;
            _inventorySettings = options.Value;
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

            return response;
        }

        public async Task<InventoryFullResponseDto?> GetPartInventoryAsync(long Id, int Count, int Page)
        {
            var res = await _inventoryRepository.GetPartByIdAsync(Id, Page, Count);
            var response = _mapper.Map<Inventory, InventoryFullResponseDto>(res); 

            return response;
        }

        public async Task<IEnumerable<InventoryResponseDto>> GetAllInventoryUserAsync(string userId)
        {
            if (userId == null)
            {
                return [];
            }

            var inventories = await _inventoryRepository.GetAllInventoryUserAsync(userId);

            return inventories.Select(i => _mapper.Map<Inventory, InventoryResponseDto>(i));
        }

        public async Task<IEnumerable<InventoryResponseDto>> GetAccessInventoryUserAsync(string userId)
        {
            if (userId == null)
            {
                return [];
            }

            IEnumerable<Inventory> inventories;

            if (await _authenticationService.IsAdmin(userId))
            {
                inventories = await _inventoryRepository.GetAllAsync();
            }
            else 
            {
                inventories = await _inventoryRepository.GetAccessInventoryUserAsync(userId);
            }

            return inventories.Select(i => _mapper.Map<Inventory, InventoryResponseDto>(i));
        }

        public async Task<InventoryResponseDto?> AddInventoryAsync(InventoryRequestDto item)
        {
            var inventory = _mapper.Map<InventoryRequestDto, Inventory>(item);

            if (string.IsNullOrEmpty(inventory.StructCustomId))
            {
                // default value
                inventory.StructCustomId = "[" +
                    "{\"id\":\"7\"," +
                    "\"name\":\"Sequence\"," +
                    "\"format\":\"\"}" +
                    "]";
            }
            
            var res = await _inventoryRepository.AddAsync(inventory);
            var resInventoryType = await _inventoryTypeRepository.AddAsync(new InventoryType() {
                Name = _inventorySettings.CustomIdName,
                Type = "string",
                InventoryId = res.Id
            });

            return _mapper.Map<Inventory, InventoryResponseDto>(res);
        }

        public async Task<ItemFullResponseDto?> AddItemAsync(ItemFullRequestDto item)
        {
            var myId = _authenticationService.MyId();
            if (!(await _authenticationService.MayEditInventory(myId, item.InventoryId)))
            {
                return null;
            }

            var itemFull = _mapper.Map<ItemFullRequestDto, Item>(item);
            var structCustomId = await _inventoryRepository.GetStructCustomIdAsync(itemFull.InventoryId); 
            var maxSequence = 1 + await _inventoryRepository.GetMaxSequenceAsync(itemFull.InventoryId);
            itemFull.Sequence = maxSequence;
            itemFull.ItemValue.Add(new ItemValue()
            {
                Value = _customIdService.GenerateCustomId(structCustomId, maxSequence),
                InventoryTypeId = await _inventoryTypeRepository.GetIdItemValueCustomIdAsync(itemFull.InventoryId)
            });

            var res = await _itemRepository.AddAsync(itemFull);

            return _mapper.Map<Item, ItemFullResponseDto>(res);
        }
        public async Task AddItemValueAsync(long inventoryId, AddItemValueRequestDto[] request)
        {
            var myId = _authenticationService.MyId();
            if (!(await _authenticationService.MayEditInventory(myId, inventoryId)))
            {
                return;
            }

            var itemValue = request.Select(r => _mapper.Map<AddItemValueRequestDto, ItemValue>(r)).ToArray();

            await _itemValueRepository.AddRangeAsync(itemValue);
        }

        public async Task<InventoryTypeResponseDto?> AddFieldAsync(InventoryTypeRequestDto item)
        {
            var myId = _authenticationService.MyId();
            if (!(await _authenticationService.MayDropAndCreateField(myId, item.InventoryId)))
            {
                return null;
            }

            var inventoryType = _mapper.Map<InventoryTypeRequestDto, InventoryType>(item);
            var res = await _inventoryTypeRepository.AddAsync(inventoryType);

            return _mapper.Map<InventoryType, InventoryTypeResponseDto>(res);
        }

        public async Task<int?> DeleteInventoryAsync(long[] idInventory)
        {
            int res = 0;

            var myId = _authenticationService.MyId();
            foreach (var id in idInventory)
            {
                if (await _authenticationService.MayDropAndCreateInventory(myId, id))
                { 
                    res += await _inventoryRepository.DeleteAsync([id]);
                }
            }

            return res;
        }

        public async Task<int?> DeleteItemsAsync(long idInventory, long[] itemsId)
        {
            var myId = _authenticationService.MyId();
            if (!(await _authenticationService.MayEditInventory(myId, idInventory)))
            {
                return null;
            }

            return await _itemRepository.DeleteAsync(itemsId);
        }

        public async Task<int?> DeleteFieldAsync(long idInventory, long[] fieldsId)
        {
            var myId = _authenticationService.MyId();
            if (!(await _authenticationService.MayEditInventory(myId, idInventory)))
            {
                return null;
            }

            return await _inventoryTypeRepository.DeleteAsync(fieldsId);
        }
        
        public async Task UpdateItemAsync(UpdateItemRequestDto[] data, long idInventory)
        {
            var myId = _authenticationService.MyId();
            if (!(await _authenticationService.MayEditInventory(myId, idInventory)))
            {
                return;
            }

            await _itemValueRepository.UpdateAsync(data);
        }

        public IEnumerable<PartNameCustomId> GetAllPartCustomId()
        {
            return _customIdService.GetAllPartCustomId();
        }

        public async Task SetCustomIdAsync(long idInventory, string structCustomId)
        {
            var myId = _authenticationService.MyId();
            if (!(await _authenticationService.MayEditCutomIdInventory(myId, idInventory)))
            {
                return;
            }

            await _inventoryRepository.UpdateCustomIdAsync(idInventory, structCustomId);
        }
    }
}