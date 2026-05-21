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
        private readonly IAuthenticationService _authenticationService;
        public HomeController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<IActionResult> MainPage()
        {
            return View();
        }

        public async Task<IActionResult> PersonalPage()
        {
            return View();
        }

        public async Task<IActionResult> Inventory(long Id, long InventoryId)
        {
            var myId = _authenticationService.MyId();
            var isCreator = await _authenticationService.IsCreatorAsync(myId, InventoryId);
            var isEditor = await _authenticationService.IsEditorAsync(myId, InventoryId);
            var model = (InventoryId, isEditor, isCreator);

            switch (Id)
            {
                case 0:
                    return View("Inventory/InventoryItems", model);
                case 1:
                    return View("Inventory/InventoryDiscussion.cshtml", model);
                case 2:
                    return View("Inventory/InventoryGeneralSettings", model);
                case 3:
                    return View("Inventory/InventoryCustomId", model);
                case 4:
                    return View("Inventory/InventoryAccessSettings", model);
                case 5:
                    return View("Inventory/InventoryCustomFields", model);
                case 6:
                    return View("Inventory/InventoryStatistics", model);
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
