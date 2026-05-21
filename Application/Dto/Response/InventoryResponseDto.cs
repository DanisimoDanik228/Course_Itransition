using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto.Response
{
    public class InventoryResponseDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatorName { get; set; }
    }
}
