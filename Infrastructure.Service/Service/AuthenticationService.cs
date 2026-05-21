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

            if (await IsPublicInventoryAsync(inventoryId)) {
                var isExistUser = await _userManager.Users.AnyAsync(u => u.Id == userId);

                return isExistUser;
            }

            var res = await _context.Inventory
                .AnyAsync(i => i.Id == inventoryId &&
                    (i.CreatorId == userId ||
                     _context.EditorInventory.Any(ei => ei.InventoryId == inventoryId && ei.EditorId == userId)));

            return res;
        }
        public async Task<bool> IsCreatorAsync(string userId, long inventoryId)
        {
            if (userId == null)
            {
                return false;
            }
            var inventory = await _context.Inventory.FindAsync(inventoryId);
            if (inventory == null) 
            {
                return false;
            }

            return inventory.CreatorId == userId;
        }

        public async Task<bool> IsEditorAsync(string userId, long inventoryId)
        {
            if (userId == null)
            {
                return false;
            }

            return await _context.EditorInventory.AnyAsync(ei => ei.InventoryId == inventoryId && ei.EditorId == userId);
        }

        public async Task<bool> MayDropAndCreateField(string userId, long inventoryId)
        {
            if (await _userManager.IsInRoleAsync(new AppUser { Id = userId }, "Admin"))
            { 
                return true;
            }

            var res = await _context.Inventory
                .AnyAsync(i => i.Id == inventoryId &&
                    (i.CreatorId == userId));

            return res;
        }

        public async Task<bool> MayDropAndCreateInventory(string userId, long inventoryId)
        {
            if (await _userManager.IsInRoleAsync(new AppUser { Id = userId }, "Admin"))
            {
                return true;
            }

            var res = await _context.Inventory
                .AnyAsync(i => i.Id == inventoryId &&
                    (i.CreatorId == userId));

            return res;
        }

        public async Task<bool> IsAdmin(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return await _userManager.IsInRoleAsync(user, "Admin");
        }

        public string MyId()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public async Task<bool> MayDropAndCreateEditor(string userId, long inventoryId)
        {
            if (await _userManager.IsInRoleAsync(new AppUser { Id = userId }, "Admin"))
            {
                return true;
            }

            var res = await _context.Inventory
                .AnyAsync(i => i.Id == inventoryId &&
                    (i.CreatorId == userId));

            return res;
        }

        public async Task<bool> MayEditCutomIdInventory(string userId, long inventoryId)
        {
            if (await _userManager.IsInRoleAsync(new AppUser { Id = userId }, "Admin"))
            {
                return true;
            }

            var res = await _context.Inventory
                .AnyAsync(i => i.Id == inventoryId &&
                    (i.CreatorId == userId));

            return res;
        }

        private async Task<bool> IsPublicInventoryAsync(long inventoryId) {
            var inventory = await _context.Inventory
                .FindAsync(inventoryId);

            return inventory.IsPublic;
        }
    }
}
