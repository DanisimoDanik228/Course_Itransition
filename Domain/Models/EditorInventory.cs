using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class EditorInventory
    {
        public long Id { get; set; }
        public string EditorId { get; set; }
        public AppUser Editor { get; set; }
        public long InventoryId { get; set; }
        public Inventory Inventory { get; set; }
    }
}
