using Application.Dto.Response;
using Application.Repository.User;
using Domain.Models;
using Infrastructure.Repository.PostgresDbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Infrastructure.Repository.Repository.User
{
    public class EditorRepository : IEditorRepository
    {
        private const int _countFindEditor = 10;
        private readonly AppDbContext _context;

        public EditorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool?> MakeEditorAsync(string[] userId, long inventoryId)
        {
            var existUser = await _context.EditorInventory
                .Where(i => i.InventoryId == inventoryId && userId.Contains(i.EditorId))
                .Select(i => i.EditorId)
                .ToListAsync();

            userId = userId.Where(u => !existUser.Contains(u)).ToArray();

            var inventory = await _context.Inventory
                .Include(i => i.Editors)
                .Where(i => i.Id == inventoryId)
                .FirstOrDefaultAsync();

            foreach (var item in userId)
            {
                inventory.Editors.Add(new EditorInventory() { EditorId = item, InventoryId = inventoryId });
            }

            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool?> RemoveEditorAsync(string[] userId, long inventoryId)
        {
            await _context.EditorInventory
                .Where(u => u.InventoryId == inventoryId && userId.Contains(u.EditorId))
                .ExecuteDeleteAsync();

            return true;
        }

        public async Task<List<InventoryEditorResponseDto>> GetEditorInventoryAsync(long inventoryId)
        {
            var adminRole = await _context.Roles
                .Where(r => r.Name == "Admin")
                .AsNoTracking()
                .FirstOrDefaultAsync();

            var admins = _context.UserRoles
                .Where(ur => adminRole.Id == ur.RoleId)
                .Select(ur => ur.UserId);

            var inventory = await _context.Inventory
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == inventoryId);

            var users = await _context.Users
                .AsNoTracking()
                .Select(u => new InventoryEditorResponseDto() { 
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    RoleInventory = (u.Id == inventory.CreatorId) ? ("Creator") : 
                        ((admins.Any(id => id == u.Id)) ? ("Admin") : 
                        ((_context.EditorInventory.Any(e => e.InventoryId == inventoryId && e.EditorId == u.Id)) ? ("Editor") : 
                        ("Anonym")))
                })
                .ToListAsync();

            return users;
        }

        public async Task<IEnumerable<string>> GetEditorRolesAsync(long inventoryId, IEnumerable<string> userIds)
        {
            var adminRole = await _context.Roles
                .Where(r => r.Name == "Admin")
                .AsNoTracking()
                .FirstOrDefaultAsync();

            var admins = _context.UserRoles
                .Where(ur => adminRole.Id == ur.RoleId)
                .Select(ur => ur.UserId);

            var inventory = await _context.Inventory
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == inventoryId);

            return userIds.Select(u => (u == inventory.CreatorId) ? ("Creator") :
                        ((admins.Any(id => id == u)) ? ("Admin") :
                        ((_context.EditorInventory.Any(e => e.InventoryId == inventoryId && e.EditorId == u)) ? ("Editor") :
                        ("Anonym"))));
        }
    }
}
