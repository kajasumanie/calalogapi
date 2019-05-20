
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalalogAPI
{
    public static class DocumentDBRepository<T> where T : class
    {
        private static readonly string Endpoint = "https://5f768b7c-0ee0-4-231-b9ee.documents.azure.com:443/";
        private static readonly string Key = "ksDRnUemIzfyWpKWpXIkNKAfmS6ibIC7rR14tmo5gDNeNTjLyZ5WqdA8qAJ3LOCuRtorg4znJSogjVbFxydMzw==";

        internal static Task GetItemsAsync(object collectionId)
        {
            throw new NotImplementedException();
        }

     private static readonly string DatabaseId = "Catalog";
     private static readonly string BookCollectionId = "CalalogDB";
        private static DocumentClient client;
        public static void Initialize()
        {
            client = new DocumentClient(new Uri(Endpoint), Key, new ConnectionPolicy
            {
                EnableEndpointDiscovery = false
            });
            CreateDatabaseIfNotExistsAsync().Wait();
            CreateCollectionIfNotExistsAsync(BookCollectionId).Wait();
        }
        private static async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDatabaseAsync(new Database
                    {
                        Id = DatabaseId
                    });
                }
                else
                {
                    throw;
                }
            }
        }
        private static async Task CreateCollectionIfNotExistsAsync(string collectionId)
        {
            try
            {
                await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, collectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDocumentCollectionAsync(UriFactory.CreateDatabaseUri(DatabaseId), new DocumentCollection
                    {
                        Id = collectionId
                    }, new RequestOptions
                    {
                        OfferThroughput = 1000
                    });
                }
                else
                {
                    throw;
                }
            }
        }
        public static async Task<T> GetSingleItemAsync(string id, string collectionId)
        {
            try
            {
                Document document = await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, collectionId, id));
                return (T)(dynamic)document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }
        public static async Task<IEnumerable<T>> GetItemsAsync(string collectionId)
        {
            IDocumentQuery<T> query = client.CreateDocumentQuery<T>(UriFactory.CreateDocumentCollectionUri(DatabaseId, collectionId), new FeedOptions
            {
                MaxItemCount = -1
            }).AsDocumentQuery();
            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }
            return results;
        }
        public static async Task<Document> CreateItemAsync(T item, string collectionId)
        {
            return await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, collectionId), item);
        }
        public static async Task<Document> UpdateItemAsync(string id, T item, string collectionId)
        {
            return await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, collectionId, id), item);
        }
        public static async Task DeleteItemAsync(string id, string collectionId)
        {
            await client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, collectionId, id));
        }
    }
}