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

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Inventory(long idInventory)
        {            
            return View(idInventory);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddInventory([FromBody] InventoryRequestDto inventory)
        {
            var i = new Inventory() { Id = inventory.Id, Name = inventory.Name, InventoryType = [], Items = [] };

            var res = await _service.AddInventoryAsync(i);
            return Json(res);
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

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllInventory()
        {
            return Json(await _service.GetAllInventoryAsync());
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetFullInventory(long idInventory)
        {
            var res = await _service.GetFullInventoryByIdAsync(idInventory);

            res = _service.PrepareFullInventoryToShow(res);
            return Json(res);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteInventory([FromBody] long[] Ids)
        {
            await _service.DeleteInventoryAsync(Ids);

            return StatusCode(204);
        }
    }
}
