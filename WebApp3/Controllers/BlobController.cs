using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using WebApp3;

namespace WebApp3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlobController : ControllerBase
    {
        private readonly AzureBlobStorageSettings _blobSettings;
        private const string DefaultFileName = "message-b82054fd-38e9-4f8c-bed5-9f00ca62df2a.txt"; // Default file name

        // Constructor to inject the settings
        public BlobController(IOptions<AzureBlobStorageSettings> blobSettings)
        {
            _blobSettings = blobSettings.Value;
        }

        // POST: api/echo/save
        [HttpPost("save")]
        public async Task<IActionResult> SaveToBlobAsync([FromBody] MessageModel model)
        {
            try
            {
                // Save the message to Azure Blob Storage
                await SaveMessageToBlobAsync(model.Message);

                // Return success message
                return Ok(new { message = "Message saved to blob storage successfully." });
            }
            catch (Exception ex)
            {
                // Handle any errors during blob storage upload
                return StatusCode(500, new { error = "An error occurred while saving the message to blob storage.", details = ex.Message });
            }
        }

        private async Task SaveMessageToBlobAsync(string message)
        {
            // Create a BlobServiceClient using settings from appsettings.json
            BlobServiceClient blobServiceClient = new BlobServiceClient(_blobSettings.ConnectionString);

            // Get a reference to the container
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_blobSettings.ContainerName);

            // Ensure the container exists (this will create the container if it does not exist)
            await containerClient.CreateIfNotExistsAsync();

            // Generate a unique name for the blob (you can customize this)
            string blobName = $"message-{Guid.NewGuid()}.txt";

            // Get a reference to the blob
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            // Convert the message to a byte array
            byte[] byteArray = Encoding.UTF8.GetBytes(message);

            // Upload the message to the blob
            using (var stream = new MemoryStream(byteArray))
            {
                await blobClient.UploadAsync(stream, true);
            }
        }

        // GET: api/echo/blob (will use default file if no fileName is specified)
        [HttpGet("blob")]
        public async Task<IActionResult> GetBlobAsync(string fileName = null)
        {
            try
            {
                // Use the default file name if no fileName is provided
                fileName ??= DefaultFileName;

                // Create a BlobServiceClient using settings from appsettings.json
                BlobServiceClient blobServiceClient = new BlobServiceClient(_blobSettings.ConnectionString);

                // Get a reference to the container
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_blobSettings.ContainerName);

                // Get a reference to the blob (file)
                BlobClient blobClient = containerClient.GetBlobClient(fileName);

                // Check if the blob exists
                if (!await blobClient.ExistsAsync())
                {
                    return NotFound(new { message = $"File '{fileName}' not found." });
                }

                // Download the blob's content
                BlobDownloadInfo download = await blobClient.DownloadAsync();

                // Return the file as a stream (you can adjust this to return as a file download)
                return File(download.Content, download.ContentType, fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while retrieving the file from blob storage.", details = ex.Message });
            }
        }
    }
}
