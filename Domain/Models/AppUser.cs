using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public ICollection<Inventory> CreatedInventory { get; set; } 
        public ICollection<EditorInventory> EditInventory { get; set; } 
    }
}
