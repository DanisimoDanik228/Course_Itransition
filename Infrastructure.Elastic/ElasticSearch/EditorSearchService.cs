using Application.Dto.Response;
using Application.Service;
using Domain.Models;
using Infrastructure.Elastic.EditorModel;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Elastic.ElasticSearch
{
    public class EditorSearchService : IEditorSearchService
    {
        private const string editorIndex = "editor_index";
        private readonly ElasticsearchClient _elasticsearchClient;

        public EditorSearchService(ElasticsearchClient elasticsearchClient)
        {
            _elasticsearchClient = elasticsearchClient;
        }

        public async Task<List<InventoryEditorResponseDto>> FindEditorInventoryByEmailAsync(long inventoryId, string email)
        {
            var response = await _elasticsearchClient.SearchAsync<EditorSearchModel>(e => e
                           .Index(editorIndex)
                           .Query(q => q
                               .Wildcard(w => w
                               .Field(f => f.Email)
                               .Value($"*{email.ToLower()}*")
                               )
                           )
                       );

            if (response.IsValidResponse)
            {
                return response.Documents
                    .Select(r => new InventoryEditorResponseDto()
                    {
                        Id = r.Id,
                        Name = r.UserName,
                        Email = r.Email,
                        RoleInventory = "___e____"
                    })
                    .ToList();
            }

            return [];
        }

        public async Task<List<InventoryEditorResponseDto>> FindEditorInventoryByNameAsync(long inventoryId, string userName)
        {
            var response = await _elasticsearchClient.SearchAsync<EditorSearchModel>(e => e
                .Index(editorIndex)
                .Query(q => q
                    .Wildcard(w => w
                    .Field(f => f.UserName)
                    .Value($"*{userName.ToLower()}*")
                    )
                )
            );

            if (response.IsValidResponse)
            {
                return response.Documents
                    .Select(r => new InventoryEditorResponseDto() {
                        Id = r.Id,
                        Name = r.UserName,
                        Email = r.Email,
                        RoleInventory = "_______"
                    })
                    .ToList();
            }

            return [];
        }

        public async Task IndexUserAsync(AppUser user)
        {
            var model = new EditorSearchModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };

            await _elasticsearchClient.IndexAsync(model, i => i.Index(editorIndex).Id(model.Id));
        }

        public async Task DeleteUserAsync(string[] userIds)
        {
            foreach (var userId in userIds)
                await _elasticsearchClient.DeleteAsync<EditorSearchModel>(userId, d => d.Index(editorIndex));
        }
    }
}
