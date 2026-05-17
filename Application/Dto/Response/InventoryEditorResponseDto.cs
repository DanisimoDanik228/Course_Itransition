using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto.Response
{
    public class InventoryEditorResponseDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string RoleInventory { get; set; } //Admin, Editor, Creator(You), Anonym
    }
}
