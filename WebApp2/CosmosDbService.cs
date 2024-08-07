using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace WebApp2
{
    public class CosmosDbService
    {
        private readonly CosmosClient _cosmosClient;
        private Database _database;
        private Container _container;

        public CosmosDbService(IOptions<CosmosDbSettings> cosmosDbSettings)
        {
            var settings = cosmosDbSettings.Value;
            _cosmosClient = new CosmosClient(settings.EndpointUri, settings.PrimaryKey);
            InitializeAsync(settings.DatabaseId, settings.ContainerId).Wait();
        }

        private async Task InitializeAsync(string databaseId, string containerId)
        {
            _database = await _cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            _container = await _database.CreateContainerIfNotExistsAsync(containerId, "/partitionKey");
        }

        public async Task<Family> GetFamilyByIdAsync(string id, string partitionKey)
        {
            try
            {
                ItemResponse<Family> response = await _container.ReadItemAsync<Family>(id, new PartitionKey(partitionKey));
                return response.Resource;
            }
            catch (CosmosException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine($"Item with id: {id} not found.");
                    return null;
                }
                throw;
            }
        }

        public async Task<Family> CreateFamilyAsync(Family family)
        {
            try
            {
                ItemResponse<Family> response = await _container.CreateItemAsync(family, new PartitionKey(family.PartitionKey));
                return response.Resource;
            }
            catch (CosmosException ex)
            {
                // Handle creation exception
                throw new Exception($"Failed to create family: {ex.Message}");
            }
        }
    }
}
