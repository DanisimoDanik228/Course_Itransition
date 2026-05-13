using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto.Request
{
    public class ItemValueRequestDto
    {
        public long Id { get; set; }

        public string Value { get; set; }
        public long InventoryTypeId { get; set; }
    }
}
