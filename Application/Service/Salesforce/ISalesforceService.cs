using Domain.Models.Salesforce;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Service.Salesforce
{
    public interface ISalesforceService
    {
        Task<IEnumerable<ContactRecord>> GetAllContactsAsync();
        Task<ContactRecord> GetContactByIdAsync(string id);
        Task<string> AddContactAsync(string email, string name);
        Task<string> GetContactIdByEmailAsync(string email);
    }
}
