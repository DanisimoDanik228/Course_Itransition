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
            return View(idInventory);
        }
    }
}
