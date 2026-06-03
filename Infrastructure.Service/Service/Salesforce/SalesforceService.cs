using Application.Options;
using Application.Service.Salesforce;
using Domain.Models.Salesforce;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Service.Service.Salesforce
{
    public class SalesforceService : ISalesforceService
    {
        public static readonly HttpClient Client = new HttpClient();
        private readonly SalesforceSettings _salesforceSettings;
        public SalesforceService(IOptions<SalesforceSettings> options)
        {
            _salesforceSettings = options.Value;

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _salesforceSettings.AccessToken);
        }

        public async Task<string> AddContactAsync(string email, string name)
        {
            string url = $"{_salesforceSettings.EndpointUrl}/services/data/{_salesforceSettings.ApiVersion}/sobjects/{_salesforceSettings.SObjectName}";

            var contact = new Contact() {LastName=name,Email=email };
            string jsonBody = JsonSerializer.Serialize(contact);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _salesforceSettings.AccessToken);
            request.Content = content;

            var response = await Client.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var res = JsonSerializer.Deserialize<CreateResponse>(result);

                if (res.success)
                {
                    return res.id;
                }
                else
                { 
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<ContactRecord>> GetAllContactsAsync()
        {
            throw new NotImplementedException();

            List<ContactRecord> res = new();

            string soql = "SELECT Id, FirstName, Email, Birthdate, Description FROM Contact";
            string url = $"{_salesforceSettings.EndpointUrl}/services/data/v60.0/query/?q={Uri.EscapeDataString(soql)}";

            bool isDone = false;

            while (!isDone)
            {
                var response = await Client.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var data = JsonSerializer.Deserialize<SalesforceResponse>(json);

                    res.AddRange(data.records);

                    isDone = data.done;
                    if (!isDone)
                    {
                        url = $"{_salesforceSettings.EndpointUrl}{data.nextRecordsUrl}";
                    }
                }
            }

            return res;
        }

        public async Task<string> GetContactIdByEmailAsync(string email)
        {
            string soql = $"SELECT Id FROM Contact WHERE Email = '{email}' LIMIT 1";

            string encodedQuery = Uri.EscapeDataString(soql);
            string url = $"{_salesforceSettings.EndpointUrl}/services/data/v60.0/query/?q={encodedQuery}";

            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _salesforceSettings.AccessToken);

            var response = await Client.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<SalesforceIdResponse>(responseBody);

                if (result.totalSize > 0 && result.records.Count > 0)
                {
                    return result.records[0];
                }

                return null; 
            }
            else
            {
                return null;
            }
        }

        public async Task<ContactRecord> GetContactByIdAsync(string id)
        {
            string[] fields = { "Id", "LastName", "Email", "Birthdate", "Description" };
            string fieldsList = string.Join(",", fields);

            string url = $"{_salesforceSettings.EndpointUrl}/services/data/{_salesforceSettings.ApiVersion}/sobjects/{_salesforceSettings.SObjectName}/{id}?fields={fieldsList}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _salesforceSettings.AccessToken);

            var response = await Client.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var contact = JsonSerializer.Deserialize<ContactRecord>(result);
                return contact;
            }
            else
            {
                return null;
            }
        }

    }
}
