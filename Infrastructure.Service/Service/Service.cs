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
using System.Text.Json;
using System.Text.Json.Serialization;

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
            return (await _inventoryRepository.GetAllInventoryWithCreatorNameAsync()).Select(i => _mapper.Map<Inventory,InventoryResponseDto>(i));
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

        public async Task<InventoryFullResponseDto?> GetPartInventoryAsync(long Id, int Page)
        {
            int Count = _inventorySettings.CountItemPerPage;
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
        public async Task<IEnumerable<InventoryTypeResponseDto>> GetInventoryTypesAsync(long inventoryId)
        {
            var res = await _inventoryRepository.GetInventoryTypeOnInventoryAsync(inventoryId);
            
            var res1 =  res.Select(i => _mapper.Map<InventoryType, InventoryTypeResponseDto>(i));
            return res1;
        }
        public async Task<int[]> GetOrderField(long inventoryId) 
        {
            return await _inventoryRepository.GetOrderField(inventoryId);
        }
        public async Task<InventoryResponseDto?> AddInventoryAsync(InventoryRequestDto item)
        {
            var inventory = _mapper.Map<InventoryRequestDto, Inventory>(item);

            if (string.IsNullOrEmpty(inventory.StructCustomId))
            {
                inventory.StructCustomId = _inventorySettings.DefaultStructCustomId;
            }

            inventory.OrderField = [1];
            inventory.IsPublic = false;
            inventory.Description = "";
            var res = await _inventoryRepository.AddAsync(inventory);
            var resInventoryType = await _inventoryTypeRepository.AddAsync(new InventoryType() {
                Name = _inventorySettings.CustomIdName,
                Type = FieldType.SingleLine,
                Description = "",
                IsShowInventoryTab = false,
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
            var id = await _inventoryTypeRepository.GetIdItemValueCustomIdAsync(itemFull.InventoryId); 
            var maxSequence = 1 + await _inventoryRepository.GetMaxSequenceAsync(itemFull.InventoryId);
            itemFull.Sequence = maxSequence;
            string customId = null;
            
            for (int i = 0; i < _inventorySettings.TryGenerateCustomId; i++)
            {
                var tempCustomId = _customIdService.GenerateCustomId(structCustomId, maxSequence);
                var exist = await _itemValueRepository.ExistCustomIdAsync(id, tempCustomId);

                if (!exist)
                {
                    customId = tempCustomId;
                    break;
                }
            }

            if (customId == null) {
                return null;
            }

            itemFull.ItemValue.Add(new ItemValue()
            {
                Value = customId,
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
        
        public async Task UpdateItemAsync(List<UpdateItemRequestDto> data, long idInventory)
        {
            var myId = _authenticationService.MyId();
            if (!(await _authenticationService.MayEditInventory(myId, idInventory)))
            {
                return;
            }

            var id = await _inventoryTypeRepository.GetIdItemValueCustomIdAsync(idInventory);
            var itemCustomId = data.Where(d => d.InventoryTypeId == id).FirstOrDefault();
            if (itemCustomId != null)
            {
                var structCustomId = await _inventoryRepository.GetStructCustomIdAsync(idInventory);
                var sequence = await _inventoryRepository.GetMaxSequenceAsync(idInventory);
                var valid = _customIdService.IsValidCustomId(structCustomId, itemCustomId.Value, sequence);

                if (!valid)
                {
                    data.Remove(itemCustomId);
                }
                else
                {
                    var exist = await _itemValueRepository.ExistCustomIdAsync(id, itemCustomId.Value);

                    if (exist)
                    {
                        data.Remove(itemCustomId);
                    }
                }
            }

            await _itemValueRepository.UpdateAsync(data);
        }

        public IEnumerable<PartNameCustomId> GetAllPartCustomId()
        {
            return _customIdService.GetAllPartCustomId();
        }

        public async Task UpdateStructCustomIdAsync(long idInventory, List<PartCustomId> structCustomId)
        {
            if (!structCustomId.Any())
            {
                return;
            }

            var myId = _authenticationService.MyId();
            if (!(await _authenticationService.MayEditCutomIdInventory(myId, idInventory)))
            {
                return;
            }

            var str = JsonSerializer.Serialize(structCustomId);

            await _inventoryRepository.UpdateCustomIdAsync(idInventory, str);
        }

        public async Task<List<PartCustomId>> GetStructCustomIdAsync(long inventoryId)
        {
            var str = await _inventoryRepository.GetStructCustomIdAsync(inventoryId);
            return JsonSerializer.Deserialize<List<PartCustomId>>(str, new JsonSerializerOptions{PropertyNameCaseInsensitive = true});
        }

        public async Task UpdateInvertoryTypes(List<InventoryTypeRequestDto> data)
        {
            var setId = data.Select(i => i.InventoryId).ToHashSet();
            if (setId.Count() != 1) {
                return;
            }

            var invertoryId = setId.First();
            var myId = _authenticationService.MyId();
            if (!(await _authenticationService.MayEditCutomIdInventory(myId, invertoryId)))
            {
                return;
            }

            var id = await _inventoryTypeRepository.GetIdItemValueCustomIdAsync(invertoryId);
            foreach (var item in data)
            {
                if (item.Id == id) 
                {
                    item.Name = _inventorySettings.CustomIdName;
                    item.Type = (int)FieldType.SingleLine;
                }
            }

            var inventoryTypes = data.Select(i => _mapper.Map<InventoryTypeRequestDto,InventoryType>(i));
            await _inventoryTypeRepository.UpdateRangeAsync(inventoryTypes);
        }

        public async Task UpdateOrderField(long inventoryId, int[] orderField)
        {
            await _inventoryRepository.UpdateOrderFieldAsync(inventoryId, orderField);
        }

        public async Task<bool> GetStatusAsync(long inventoryId)
        {
            return await _inventoryRepository.GetStatusAsync(inventoryId);
        }

        public async Task UpdateStatusInventoryAsync(long inventoryId, bool status)
        {
            await _inventoryRepository.UpdateStatusInventoryAsync(inventoryId, status);
        }
    }
}