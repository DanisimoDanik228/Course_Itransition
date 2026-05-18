using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto.Request
{
    public class UpdateItemRequestDto
    {
        public long ItemValueId { get; set; }
        public string Value { get; set; }
    }
}
