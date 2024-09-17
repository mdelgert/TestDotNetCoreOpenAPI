using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;
using WebApp3.Models;

namespace WebApp3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlobController : ControllerBase
    {
        private readonly AzureBlobModel _blobSettings;
        private const string DefaultFileName = "test.json"; // Default file name, now a .json file

        // Constructor to inject the settings
        public BlobController(IOptions<AzureBlobModel> blobSettings)
        {
            _blobSettings = blobSettings.Value;
        }

        // POST: api/echo/save
        [HttpPost("save")]
        public async Task<IActionResult> SaveToBlobAsync([FromBody] MessageModel model)
        {
            try
            {
                // Save the message to Azure Blob Storage as JSON
                await SaveMessageToBlobAsync(model, DefaultFileName);

                // Return success message with saved data
                return Ok(new { message = model.Message });
            }
            catch (Exception ex)
            {
                // Handle any errors during blob storage upload
                return StatusCode(500, new { error = "An error occurred while saving the message to blob storage.", details = ex.Message });
            }
        }

        // POST: api/echo/update
        [HttpPost("update")]
        public async Task<IActionResult> UpdateBlobAsync([FromBody] MessageModel model)
        {
            try
            {
                // Update the message by overwriting the existing blob
                await SaveMessageToBlobAsync(model, DefaultFileName);

                // Return success message with updated data
                return Ok(new { message = model.Message });
            }
            catch (Exception ex)
            {
                // Handle any errors during blob storage update
                return StatusCode(500, new { error = "An error occurred while updating the message in blob storage.", details = ex.Message });
            }
        }

        // GET: api/echo/blob (returns the default file if no fileName is provided)
        [HttpGet("blob")]
        public async Task<IActionResult> GetBlobAsync()
        {
            try
            {
                // Create a BlobServiceClient using settings from appsettings.json
                BlobServiceClient blobServiceClient = new BlobServiceClient(_blobSettings.ConnectionString);

                // Get a reference to the container
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_blobSettings.ContainerName);

                // Get a reference to the blob (file)
                BlobClient blobClient = containerClient.GetBlobClient(DefaultFileName);

                // Check if the blob exists
                if (!await blobClient.ExistsAsync())
                {
                    return NotFound(new { message = $"File '{DefaultFileName}' not found." });
                }

                // Download the blob's content
                BlobDownloadInfo download = await blobClient.DownloadAsync();

                // Read the content as a string
                string content;
                using (var reader = new StreamReader(download.Content))
                {
                    content = await reader.ReadToEndAsync();
                }

                // Deserialize JSON to MessageModel
                var messageModel = JsonConvert.DeserializeObject<MessageModel>(content);

                // Return the JSON content
                return Ok(messageModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while retrieving the file from blob storage.", details = ex.Message });
            }
        }

        private async Task SaveMessageToBlobAsync(MessageModel messageModel, string blobName)
        {
            // Create a BlobServiceClient using settings from appsettings.json
            BlobServiceClient blobServiceClient = new BlobServiceClient(_blobSettings.ConnectionString);

            // Get a reference to the container
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_blobSettings.ContainerName);

            // Ensure the container exists (this will create the container if it does not exist)
            await containerClient.CreateIfNotExistsAsync();

            // Get a reference to the blob
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            // Serialize the message model to JSON
            string jsonString = JsonConvert.SerializeObject(messageModel);

            // Convert the JSON string to a byte array
            byte[] byteArray = Encoding.UTF8.GetBytes(jsonString);

            // Upload the JSON message to the blob
            using (var stream = new MemoryStream(byteArray))
            {
                await blobClient.UploadAsync(stream, true);
            }
        }
    }
}
