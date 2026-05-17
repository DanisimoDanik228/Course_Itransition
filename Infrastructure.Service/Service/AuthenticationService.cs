using Application.Service;
using Domain.Models;
using Infrastructure.Repository.PostgresDbContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Service.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private AppDbContext _context;
        private UserManager<AppUser> _userManager;
        private IHttpContextAccessor _httpContextAccessor;
        public AuthenticationService(
            AppDbContext context,
            UserManager<AppUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> MayEditInventory(string userId, long inventoryId)
        {
            if (await _userManager.IsInRoleAsync(new AppUser { Id = userId }, "Admin"))
            { 
                return true;
            }

            var res = await _context.Inventory
                .AnyAsync(i => i.Id == inventoryId &&
                    (i.CreatorId == userId ||
                     _context.EditorInventory.Any(ei => ei.InventoryId == inventoryId && ei.EditorId == userId)));

            return res;
        }

        public string MyId()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
