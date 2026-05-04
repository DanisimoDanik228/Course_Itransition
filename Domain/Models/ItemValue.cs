using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class ItemValue
    {
        public long Id { get; set; }

        public string Value { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }

        public Item Item { get; set; }
    }
}
