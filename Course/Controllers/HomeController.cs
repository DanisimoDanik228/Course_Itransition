using Application.Dto.Request;
using Application.Service;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
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
            return View(await _service.GetAllInventoryAsync());
        }
        
        public async Task<IActionResult> Inventory(long idInventory)
        {
            var res = await _service.GetAllItemsFromInventoryAsync(idInventory);
            var res1 = res.ToList();
            return View(res);
        }

        [HttpPost]
        public async Task<IActionResult> AddInventory([FromBody] InventoryRequestDto inventory)
        {
            var it1 = new InventoryType() {Id=1, Type="string", Name="Name" };
            var it2 = new InventoryType() {Id=2, Type="int",Name="Age" };

            var iv1 = new ItemValue() {Id=1,Value="19",Type="int" };
            var iv2 = new ItemValue() {Id=2,Value="danila",Type="string" };
            var iv3 = new ItemValue() {Id=3,Value="23",Type="int" };
            var iv4 = new ItemValue() {Id=4,Value="kirill",Type="string" };

            var i1 = new Item() { Id = 1, ItemValue = [iv1,iv2] };
            var i2 = new Item() { Id = 2, ItemValue = [iv3,iv4] };

            i1.ItemValue = [iv1, iv2];
            i2.ItemValue = [iv3, iv4];

            var i = new Inventory() { Id = inventory.Id, Name = inventory.Name};

            i.Items = [i1,i2];
            i.InventoryType = [it1,it2];

            var res = await _service.AddInventoryAsync(i);
            return Json(res);
        }
    }
}
