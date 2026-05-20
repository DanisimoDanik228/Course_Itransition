using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Options
{
    public class InventorySettings
    {
        public string CustomIdName { get; set; }
        public int TryGenerateCustomId { get; set; }
        public string DefaultStructCustomId { get; set; }
        public int CountItemPerPage { get; set; }
    }
}
