using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public enum FieldType
    {
        SingleLine,
        MultiLine,
        Numeric,
        Document,
        Bool
    }

    public class InventoryType
    {
        public long Id { get; set; }
        public FieldType Type { get; set; }
        public string Name { get; set; }
        public bool IsShowInventoryTab { get; set; }
        public string Description { get; set; }
        public long InventoryId { get; set; }
        public Inventory Inventory { get; set; }
        public ICollection<ItemValue> ItemValue { get; set; }
    }
}
