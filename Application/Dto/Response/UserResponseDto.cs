using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto.Response
{
    public class UserResponseDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Role { get; set; }
        public bool IsBlocked { get; set; }
    }
}
