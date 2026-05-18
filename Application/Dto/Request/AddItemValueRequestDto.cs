using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto.Request
{
    public class AddItemValueRequestDto
    {
        public long ItemId { get; set; } 
        public long InventoryTypeId { get; set; } 
        public string Value { get; set; }
    }
}
