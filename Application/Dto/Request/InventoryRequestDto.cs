using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto.Request
{
    public class InventoryRequestDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
