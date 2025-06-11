using System.Collections.Concurrent;
using System.ComponentModel;
using Microsoft.Azure.Cosmos;
using ProductApi.Models;

namespace ProductApi.Services
{
    public class CosmosDbService
    {
        private readonly Microsoft.Azure.Cosmos.Container _container;

        public CosmosDbService(CosmosClient client, string dbName, string containerName)
        {
            _container = client.GetContainer(dbName, containerName);
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            var query = _container.GetItemQueryIterator<Product>("SELECT * FROM c");
            List<Product> results = new();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response);
            }
            return results;
        }

        public async Task<Product> GetProductAsync(string id)
        {
            try
            {
                ItemResponse<Product> response = await _container.ReadItemAsync<Product>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch
            {
                return null;
            }
        }

        public async Task AddProductAsync(Product product)
        {
            if (string.IsNullOrEmpty(product.Id))
            {
                product.Id = Guid.NewGuid().ToString();
            }

            await _container.CreateItemAsync(product, new PartitionKey(product.Id));
        }

        public async Task UpdateProductAsync(Product product)
        {
            await _container.UpsertItemAsync(product, new PartitionKey(product.Id));
        }

        public async Task DeleteProductAsync(string id)
        {
            await _container.DeleteItemAsync<Product>(id, new PartitionKey(id));
        }
    }
}
