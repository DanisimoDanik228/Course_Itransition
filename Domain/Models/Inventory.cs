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
        public ICollection<Item> Items { get; set; }
        public ICollection<InventoryType> InventoryType { get; set; }
    }
}
