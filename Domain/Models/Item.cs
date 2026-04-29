using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class Item
    {
        public long Id { get; set; }

        public ICollection<ItemValue> ItemValue { get; set; } 

        public long InventoryId { get; set; }
        public Inventory Inventory { get; set; } 
    }
}
