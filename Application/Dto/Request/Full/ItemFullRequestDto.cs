using Application.Dto.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto.Request.Full
{
    public class ItemFullRequestDto
    {
        public long Id { get; set; }
        public long InventoryId { get; set; }

        public List<ItemValueRequestDto> ItemValue { get; set; }
    }
}
