using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public enum TypePartCustomId
    {
        FixedText,
        BitNumber20,
        BitNumber32,
        DigitNumber6,
        DigitNumber9,
        GUID,
        DateTime, //(at the moment of item creation),
        Sequence // (value equal to the largest existing sequence number +1 at the moment of item creation).
    }

    public class PartCustomId 
    {
        public string id { get; set; } // TypePartCustomId    
        public string name { get; set; }    
        public string format { get; set; }    
    }

    public class PartNameCustomId 
    {
        public int TypePartCustomId { get; set; }    
        public string Name { get; set; }    
        public string[] AvaliableFormat { get; set; }    
    }
}
