﻿using System.Net;
using AzureTestApp.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace AzureTestApp.Services
{
    public class CosmosDbService
    {
        private readonly CosmosClient _cosmosClient;
        private Container _container;
        private Database _database;

        public CosmosDbService(IOptions<CosmosDbSettingsModel> cosmosDbSettings)
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

        public async Task<NoteCosmosModel?> GetNoteByIdAsync(string id, string partitionKey)
        {
            try
            {
                var response = await _container.ReadItemAsync<NoteCosmosModel>(id, new PartitionKey(partitionKey));
                return response.Resource;
            }
            catch (CosmosException ex)
            {
                if (ex.StatusCode != HttpStatusCode.NotFound) throw;
                Console.WriteLine($"Item with id: {id} not found.");
                return null;
            }
        }

        public async Task<NoteCosmosModel> CreateNoteAsync(NoteCosmosModel note)
        {
            try
            {
                var response = await _container.CreateItemAsync(note, new PartitionKey(note.PartitionKey));
                return response.Resource;
            }
            catch (CosmosException ex)
            {
                // Handle creation exception
                throw new Exception($"Failed to create family: {ex.Message}");
            }
        }

        public async Task DeleteNoteAsync(string id, string partitionKey)
        {
            try
            {
                await _container.DeleteItemAsync<NoteCosmosModel>(id, new PartitionKey(partitionKey));
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                throw new Exception("Note not found");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete note: {ex.Message}");
            }
        }
    }
}