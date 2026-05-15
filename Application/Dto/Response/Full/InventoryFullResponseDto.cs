using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto.Response.Full
{
    public class InventoryFullResponseDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string CreatorName { get; set; }
        public List<ItemFullResponseDto> Items { get; set; }
        public List<InventoryTypeResponseDto> InventoryType { get; set; }
    }
}
