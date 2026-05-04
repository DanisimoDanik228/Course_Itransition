using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto.Response.Full
{
    public class ItemFullResponseDto
    {
        public long Id { get; set; }
        public long InventoryId { get; set; }

        public List<ItemValueResponseDto> ItemValue { get; set; }
    }
}
