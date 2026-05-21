using Application.Dto.Response;
using Application.Options;
using Application.Repository.User;
using Application.Service;
using Domain.Models;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Nodes;
using Infrastructure.Elastic.EditorModel;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Elastic.ElasticSearch
{
    public class EditorSearchService : IEditorSearchService
    {
        private readonly ElasticsearchClient _elasticsearchClient;
        private readonly IEditorRepository _editorRepository;
        private readonly ElasticsearchSettings _elasticsearchSettings;

        public EditorSearchService(
            ElasticsearchClient elasticsearchClient,
            IEditorRepository editorRepository,
            IOptions<ElasticsearchSettings> options)
        {
            _editorRepository = editorRepository;
            _elasticsearchClient = elasticsearchClient;
            _elasticsearchSettings = options.Value;
        }

        public async Task<List<InventoryEditorResponseDto>> FindEditorInventoryByEmailAsync(long inventoryId, string email)
        {
            var response = await _elasticsearchClient.SearchAsync<EditorSearchModel>(e => e
                    .Index(_elasticsearchSettings.DefaultIndex)
                    .Query(q => q
                        .Wildcard(w => w
                        .Field(f => f.Email)
                        .Value($"*{email.ToLower()}*")
                        )
                    )
                );

            if (response.IsValidResponse)
            {
                return await MakeEditorResponseAsync(inventoryId, response.Documents);
            }

            return [];
        }

        public async Task<List<InventoryEditorResponseDto>> FindEditorInventoryByNameAsync(long inventoryId, string userName)
        {
            var response = await _elasticsearchClient.SearchAsync<EditorSearchModel>(e => e
                .Index(_elasticsearchSettings.DefaultIndex)
                .Query(q => q
                    .Wildcard(w => w
                    .Field(f => f.UserName)
                    .Value($"*{userName.ToLower()}*")
                    )
                )
            );

            if (response.IsValidResponse)
            {
                return await MakeEditorResponseAsync(inventoryId, response.Documents);
            }

            return [];
        }

        public async Task IndexUserAsync(AppUser user)
        {
            var model = new EditorSearchModel()
            {
                Id = user.Id,
                UserName = user.Name,
                Email = user.Email
            };

            await _elasticsearchClient.IndexAsync(model, i => i.Index(_elasticsearchSettings.DefaultIndex).Id(model.Id));
        }

        public async Task DeleteUserAsync(string[] userIds)
        {
            foreach (var userId in userIds)
                await _elasticsearchClient.DeleteAsync<EditorSearchModel>(userId, d => d.Index(_elasticsearchSettings.DefaultIndex));
        }

        private async Task<List<InventoryEditorResponseDto>> MakeEditorResponseAsync(long inventoryId, IEnumerable<EditorSearchModel> response)
        {
            if (!response.Any()) {
                return [];
            }

            var res = response
                .Select(r => new InventoryEditorResponseDto()
                {
                    Id = r.Id,
                    Name = r.UserName,
                    Email = r.Email,
                    RoleInventory = "___ERROR___"
                })
                .ToList();

            var roles = await _editorRepository.GetEditorRolesAsync(inventoryId, res.Select(r => r.Id));

            var enumator1 = res.GetEnumerator();
            var enumator2 = roles.GetEnumerator();

            while (enumator1.MoveNext() && enumator2.MoveNext())
            {
                var res1 = enumator1.Current;
                res1.RoleInventory = enumator2.Current;
            }

            return res;
        }
    }
}
