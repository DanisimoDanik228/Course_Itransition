using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Domain.Models
{
    public class Inventory
    {
        public long Id { get; set;  }
        public string Name { get; set; }
        public string CreatorId { get; set; }
        public AppUser Creator { get; set; } 
        public ICollection<EditorInventory> Editors { get; set; }
        public ICollection<Item> Items { get; set; }
        public ICollection<InventoryType> InventoryType { get; set; }
        public string StructCustomId { get; set; }
    }
}
