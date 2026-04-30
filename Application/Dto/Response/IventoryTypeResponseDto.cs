using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto.Response
{
    public class IventoryTypeResponseDto
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
    }
}
