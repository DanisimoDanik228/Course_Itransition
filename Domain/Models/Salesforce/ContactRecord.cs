using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models.Salesforce
{
    public class Contact
    { 
        public string LastName { get; set; } 
        public string Email { get; set; }
    }

    public class ContactRecord
    {
        public string Id { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Birthdate { get; set; } // YYYY-MM-DD
        public string Description { get; set; }
    }

    public class SalesforceResponse
    {
        public int totalSize { get; set; }
        public bool done { get; set; }
        public List<ContactRecord> records { get; set; }
        public string nextRecordsUrl { get; set; }
    }

    public class CreateResponse
    {
        public string id { get; set; }
        public bool success { get; set; }
    }

    public class SalesforceContatResponse
    {
        public int totalSize { get; set; }
        public bool done { get; set; }
        public List<ContactRecord> records { get; set; }
    }
}