using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class ItemValue
    {
        public long Id { get; set; }
        public string Value { get; set; }
        public long InventoryTypeId { get; set; }
        public InventoryType InventoryType { get; set; }
        public long ItemId { get; set; }
        public Item Item { get; set; }
    }
}
