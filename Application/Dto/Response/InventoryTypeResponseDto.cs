using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto.Response
{
    public class InventoryTypeResponseDto
    {
        public long Id { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public bool IsShowInventoryTab { get; set; }
        public string Description { get; set; }
        public long InventoryId { get; set; }
    }
}
