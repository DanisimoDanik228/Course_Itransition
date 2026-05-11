using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto.Request
{
    public class SetStatusRequest
    {
        public string Status { get; set; }
        public string[] Ids { get; set; }
    }
}
