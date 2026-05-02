using Application.Dto.Request;
using Application.Dto.Request.Full;
using Application.Dto.Response;
using Application.Dto.Response.Full;
using Application.Service;
using Domain.Models;
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

        public async Task<IActionResult> Index()
        {
            return View();
        }
        
        public async Task<IActionResult> Inventory(long idInventory)
        {            
            return View(idInventory);
        }

        [HttpPost]
        public async Task<IActionResult> AddInventory([FromBody] InventoryRequestDto inventory)
        {
            var it1 = new InventoryType() {Id=0, Type="string", Name="Name" };
            var it2 = new InventoryType() {Id=0, Type="int",Name="Age" };

            var iv1 = new ItemValue() {Id=0,Value="19",Type="int", Name = "Age" };
            var iv2 = new ItemValue() {Id=0,Value="danila",Type="string", Name = "Name" };
            var iv3 = new ItemValue() {Id=0,Value="23",Type="int",Name="Age" };
            var iv4 = new ItemValue() {Id=0,Value="kirill",Type= "string", Name="Name" };

            var i1 = new Item() { Id = 0, ItemValue = [iv1,iv2] };
            var i2 = new Item() { Id = 0, ItemValue = [iv3,iv4] };

            i1.ItemValue = [iv1, iv2];
            i2.ItemValue = [iv3, iv4];

            var i = new Inventory() { Id = inventory.Id, Name = inventory.Name};

            i.Items = [i1,i2];
            i.InventoryType = [it1,it2];

            var res = await _service.AddInventoryAsync(i);
            return Json(res);
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(long idInventory, [FromBody] ItemFullRequestDto item)
        {
            return Json(await _service.AddItemAsync(item));
        }

        [HttpPost]
        public async Task<IActionResult> AddField([FromBody] InventoryTypeRequestDto item)
        {
            return Json(await _service.AddFieldAsync(item));
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

            res = _service.PrepareFullInventoryToShow(res);
            return Json(res);
        }
    }
}
