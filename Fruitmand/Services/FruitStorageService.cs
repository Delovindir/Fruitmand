using Azure.Storage.Blobs;
using Fruitmand.Models;
using System.Text.Json;
using System.Text;
using Fruitmand.Interfaces;

namespace Fruitmand.Services
{
    public class FruitStorageService : IFruitStorageService
    {
        private readonly BlobContainerClient _container;

        public FruitStorageService(IConfiguration config)
        {
            var connectionString = config.GetSection("AzureStorage")["ConnectionString"];
            var client = new BlobServiceClient(connectionString);
            _container = client.GetBlobContainerClient("fruitbasket");
            _container.CreateIfNotExists();
        }

        public async Task AddFruitAsync(string soortFruit, FruitItem item)
        {
            string blobName = $"{soortFruit}/{item.AankoopDatum:yyyyMMddTHHmmss}.json";
            var blob = _container.GetBlobClient(blobName);
            var json = JsonSerializer.Serialize(item);
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            await blob.UploadAsync(stream);
        }

        public async Task<FruitItem?> GetOldestFruitAsync(string soortFruit)
        {
            await foreach (var blob in _container.GetBlobsAsync(prefix: $"{soortFruit}/"))
            {
                var blobClient = _container.GetBlobClient(blob.Name);
                var response = await blobClient.DownloadContentAsync();
                return JsonSerializer.Deserialize<FruitItem>(response.Value.Content.ToString());
            }

            return null;
        }

        public async Task DeleteOldestFruitAsync(string soortFruit)
        {
            await foreach (var blob in _container.GetBlobsAsync(prefix: $"{soortFruit}/"))
            {
                await _container.DeleteBlobIfExistsAsync(blob.Name);
                return;
            }
        }

        public async Task<List<FruitItem>> GetAllFruitsAsync()
        {
            var result = new List<FruitItem>();

            await foreach (var blob in _container.GetBlobsAsync())
            {
                var blobClient = _container.GetBlobClient(blob.Name);
                var response = await blobClient.DownloadContentAsync();
                var fruit = JsonSerializer.Deserialize<FruitItem>(response.Value.Content.ToString());
                if (fruit != null) result.Add(fruit);
            }

            return result;
        }

        public async Task DeleteAllFruitsAsync()
        {
            await foreach (var blob in _container.GetBlobsAsync())
            {
                await _container.DeleteBlobIfExistsAsync(blob.Name);
            }
        }
    }
}
