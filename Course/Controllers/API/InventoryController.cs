using Application.Dto.Request;
using Application.Dto.Request.Full;
using Application.Service;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Course.Controllers.API
{
    [ApiController]
    [Route("api/inventory/[action]")]
    public class InventoryController : ControllerBase
    {
        private readonly IService _service;
        public InventoryController(IService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPartCustomId()
        {
            var parts = _service.GetAllPartCustomId();
            return Ok(parts);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInventory()
        {
            return Ok(await _service.GetAllInventoryAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetFullInventory(long idInventory)
        {
            var res = await _service.GetFullInventoryByIdAsync(idInventory);

            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetPartInventory(long idInventory, int Page)
        {
            var res = await _service.GetPartInventoryAsync(idInventory, Page);

            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserInventories(string userId)
        {
            var inventories = await _service.GetAllInventoryUserAsync(userId);

            return Ok(inventories);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUserAccessInventories(string userId)
        {
            var inventories = await _service.GetAccessInventoryUserAsync(userId);

            return Ok(inventories);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Registered")]
        public async Task<IActionResult> GetStructCustomId(long idInventory)
        {
            var res = await _service.GetStructCustomIdAsync(idInventory);

            return Ok(res);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Registered")]
        public async Task<IActionResult> GetInventoryTypes(long idInventory)
        {
            var res = await _service.GetInventoryTypesAsync(idInventory);

            return Ok(res);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Registered")]
        public async Task<IActionResult> SetStructCustomId(long idInventory, [FromBody] List<PartCustomId> structCustomId)
        {
            await _service.SetStructCustomIdAsync(idInventory, structCustomId);

            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Registered")]
        public async Task<IActionResult> AddInventory([FromBody] InventoryRequestDto inventory)
        {
            var res = await _service.AddInventoryAsync(inventory);

            return Ok(res);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Registered")]
        public async Task<IActionResult> AddItemValue(long idInventory, [FromBody] AddItemValueRequestDto[] itemValue)
        {
            await _service.AddItemValueAsync(idInventory, itemValue);

            return StatusCode(204);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Registered")]
        public async Task<IActionResult> AddItem(long idInventory, [FromBody] ItemFullRequestDto item)
        {
            return Ok(await _service.AddItemAsync(item));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Registered")]
        public async Task<IActionResult> AddField([FromBody] InventoryTypeRequestDto item)
        {
            return Ok(await _service.AddFieldAsync(item));
        }

        [HttpDelete]
        [Authorize(Roles = "Admin,Registered")]
        public async Task<IActionResult> DeleteInventory([FromBody] long[] Ids)
        {
            await _service.DeleteInventoryAsync(Ids);

            return StatusCode(204);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin,Registered")]
        public async Task<IActionResult> DeleteItems(long idInventory, [FromBody] long[] Ids)
        {
            await _service.DeleteItemsAsync(idInventory, Ids);

            return StatusCode(204);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin,Registered")]
        public async Task<IActionResult> DeleteField(long idInventory, [FromBody] long[] Ids)
        {
            await _service.DeleteFieldAsync(idInventory, Ids);

            return StatusCode(204);
        }

        [HttpPatch]
        [Authorize(Roles = "Admin,Registered")]
        public async Task<IActionResult> UpdateItem(long idInventory, [FromBody] List<UpdateItemRequestDto> request)
        {
            await _service.UpdateItemAsync(request, idInventory);

            return StatusCode(204);
        }
    }
}
