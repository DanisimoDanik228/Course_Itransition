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
        public HomeController(IService service)
        {
        }

        public async Task<IActionResult> PersonalPage()
        {
            return View();
        }

        public async Task<IActionResult> Inventory(long Id, long InventoryId)
        {
            switch (Id)
            {
                case 0:
                    return View("Inventory/InventoryItems", InventoryId);
                case 1:
                    return View("Inventory/InventoryDiscussion.cshtml", InventoryId);
                case 2:
                    return View("Inventory/InventoryGeneralSettings", InventoryId);
                case 3:
                    return View("Inventory/InventoryCustomId", InventoryId);
                case 4:
                    return View("Inventory/InventoryAccessSettings", InventoryId);
                case 5:
                    return View("Inventory/InventoryCustomFields", InventoryId);
                case 6:
                    return View("Inventory/InventoryStatistics", InventoryId);
                default:
                    return View();
            }
        }

        public async Task<IActionResult> InventoryItems(long InventoryId)
        {
            return View(InventoryId);
        }
    }
}
