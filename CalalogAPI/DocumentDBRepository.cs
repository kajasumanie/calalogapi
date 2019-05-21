using CatalogAPI.Entities;
using CatalogAPI.Helpers;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace CatalogAPI.Services
{
    public class CatalogRepository : CosmosDbSdkRepository , ICatalogRepository
    {

        public CatalogRepository(
            IOptions<AzureCosmosDbOptions> azureCosmosDbOptions) : base(azureCosmosDbOptions)
        {
        }

        public async Task<Catalog> AddAsync(
            Catalog order)
        {
            var orderContainer =
               _cosmosDatabase.Containers["Items"];

            var orderDocument =
                await orderContainer.Items.CreateItemAsync<Catalog>(
                    order.Id.ToString(),
                    order);

            return orderDocument.Resource;
        }

        public async Task<Catalog> DeleteByIdAsync(
            Guid id)
        {
            var orderContainer =
               _cosmosDatabase.Containers["Items"];

            var orderDocument =
                await orderContainer.Items.DeleteItemAsync<Catalog>(
                    id.ToString(),
                    id.ToString());

            return orderDocument.Resource;
        }

        public async Task<Catalog> FetchByIdAsync(
            Guid id)
        {
            var orderContainer =
               _cosmosDatabase.Containers["Items"];

            var orderDocument =
                await orderContainer.Items.ReadItemAsync<Catalog>(
                    id.ToString(),
                    id.ToString());

            return orderDocument.Resource;
        }

        public async Task<IEnumerable<Catalog>> FetchListAsync(
            Guid? itemId)
        {
            var orderContainer =
                _cosmosDatabase.Containers["Items"];

            var query =
                $"SELECT * FROM o";

            if (itemId.HasValue)
            {
                query += $" WHERE ARRAY_CONTAINS(o.items, {{ \"id\": \"{itemId}\" }}, true)";
            }

            var queryDefinition =
                new CosmosSqlQueryDefinition(query);

            var orders =
                orderContainer.Items.CreateItemQuery<Catalog>(queryDefinition, maxConcurrency: 2);

            var orderList = new List<Catalog>();

            while (orders.HasMoreResults)
            {
                orderList.AddRange(
                    await orders.FetchNextSetAsync());
            };

            return orderList;
        }

        public async Task<Catalog> UpdateByIdAsync(
            Guid id,
            Catalog order)
        {
            var orderContainer =
               _cosmosDatabase.Containers["Items"];

            var orderDocument =
                await orderContainer.Items.ReplaceItemAsync<Catalog>(
                    id.ToString(),
                    id.ToString(),
                    order);

            return orderDocument.Resource;
        }
    }
}
