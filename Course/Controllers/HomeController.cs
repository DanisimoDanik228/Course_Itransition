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
            return View(await _service.GetAllAsync());
        }

        public async Task<IActionResult> AddItem(Inventory item)
        {
            await _service.AddAsync(item);

            return RedirectToAction("Index");
        }
    }
}
