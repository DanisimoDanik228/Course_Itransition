using Application.Dto.Request;
using Application.Dto.Request.Full;
using Application.Dto.Response;
using Application.Dto.Response.Full;
using Application.Service;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Diagnostics;

namespace Course.Controllers
{
    public class HomeController : Controller
    {
        private readonly IService _service;
        public HomeController(IService service)
        {
            _service = service;
        }

        public async Task<IActionResult> PersonalPage()
        {
            return View();
        }

        public async Task<IActionResult> Inventory(long Id, int Page)
        {
            ViewBag.Page = Page;
            return View(Id);
        }

        public async Task<IActionResult> CustomId(long idInventory)
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPartCustomId()
        {
            var parts = _service.GetAllPartCustomId();
            return Json(parts);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInventory()
        {
            return Json(await _service.GetAllInventoryAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetFullInventory(long idInventory)
        {
            var res = await _service.GetFullInventoryByIdAsync(idInventory);

            return Json(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetPartInventory(long idInventory, int Page)
        {
            int Count = 5;
            var res = await _service.GetPartInventoryAsync(idInventory, Count, Page);

            return Json(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserInventories(string userId)
        {
            var inventories = await _service.GetAllInventoryUserAsync(userId);

            return Json(inventories);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUserAccessInventories(string userId)
        {
            var inventories = await _service.GetAccessInventoryUserAsync(userId);

            return Json(inventories);
        }
        
        [HttpPost]
        [Authorize(Roles = "Admin,Registered")]
        public async Task<IActionResult> AddInventory([FromBody] InventoryRequestDto inventory)
        {
            var res = await _service.AddInventoryAsync(inventory);

            return Json(res);
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
            return Json(await _service.AddItemAsync(item));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Registered")]
        public async Task<IActionResult> AddField([FromBody] InventoryTypeRequestDto item)
        {
            return Json(await _service.AddFieldAsync(item));
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
        public async Task<IActionResult> UpdateItem(long idInventory, [FromBody] UpdateItemRequestDto[] request)
        {
            await _service.UpdateItemAsync(request, idInventory);

            return StatusCode(204);
        }
    }
}
