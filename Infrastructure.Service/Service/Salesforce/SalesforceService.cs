using Application.Options;
using Application.Service.Salesforce;
using Domain.Models.Salesforce;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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
        }

        public async Task<string> AddContactAsync(string email, string name)
        {
            string accessToken = await GetNewAccessToken();
            string url = $"{_salesforceSettings.EndpointUrl}/services/data/{_salesforceSettings.ApiVersion}/sobjects/{_salesforceSettings.SObjectName}";

            var contact = new Contact() {LastName=name,Email=email };
            string jsonBody = JsonSerializer.Serialize(contact);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
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
            //throw new NotImplementedException();

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
                else
                {
                    break;
                }
            }

            return res;
        }

        public async Task<ContactRecord> GetContactByEmailAsync(string email)
        {
            string accessToken = await GetNewAccessToken();

            string soql = $"SELECT Id, LastName, Email, Birthdate, Description FROM Contact WHERE Email = '{email}' LIMIT 1";
            string encodedQuery = Uri.EscapeDataString(soql);
            string url = $"{_salesforceSettings.EndpointUrl}/services/data/v60.0/query/?q={encodedQuery}";

            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await Client.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<SalesforceContatResponse>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true 
                });

                if (result != null && result.totalSize > 0 && result.records.Count > 0)
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
            string accessToken = await GetNewAccessToken();
            string[] fields = { "Id", "LastName", "Email", "Birthdate", "Description" };
            string fieldsList = string.Join(",", fields);

            string url = $"{_salesforceSettings.EndpointUrl}/services/data/{_salesforceSettings.ApiVersion}/sobjects/{_salesforceSettings.SObjectName}/{id}?fields={fieldsList}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

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

        public async Task<bool> UpdateContactAsync(string contactId, string newLastName, string newDescription)
        {
            string accessToken = await GetNewAccessToken();

            if (string.IsNullOrEmpty(accessToken))
            {
                return false;
            }

            string url = $"{_salesforceSettings.EndpointUrl}/services/data/v60.0/sobjects/Contact/{contactId}";

            var updateData = new
            {
                LastName = newLastName,
                Description = newDescription
            };

            string jsonBody = JsonSerializer.Serialize(updateData);

            using var request = new HttpRequestMessage(new HttpMethod("PATCH"), url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await Client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public class TokenResponse
        {
            [JsonPropertyName("access_token")]
            public string AccessToken { get; set; }

            [JsonPropertyName("instance_url")]
            public string InstanceUrl { get; set; }
        }
        private async Task<string> GetNewAccessToken()
        {
            string tokenUrl = $"{_salesforceSettings.EndpointUrl}/services/oauth2/token";
            var tokenRequest = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", _salesforceSettings.ClientId),
                new KeyValuePair<string, string>("client_secret", _salesforceSettings.ConsumerSecret)
            });

            var tokenResponse = await Client.PostAsync(tokenUrl, tokenRequest);
            var tokenJson = await tokenResponse.Content.ReadAsStringAsync();

            if (!tokenResponse.IsSuccessStatusCode)
            {
                return null;
            }

            var authResult = JsonSerializer.Deserialize<TokenResponse>(tokenJson);
            string accessToken = authResult.AccessToken;

            return accessToken;
        }
    }
}
