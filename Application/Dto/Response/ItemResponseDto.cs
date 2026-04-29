using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto.Response
{
    public class ItemResponseDto
    {
        public long Id { get; set; }

        public ICollection<ItemValue> ItemValue { get; set; }

        public long InventoryId { get; set; }
    }
}
