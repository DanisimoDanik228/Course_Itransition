using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class InventoryType
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public long InventoryId { get; set; }
        public Inventory Inventory { get; set; }
        public ICollection<ItemValue> ItemValue { get; set; }
    }
}
