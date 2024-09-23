using Azure;
using Azure.Data.Tables;
using AzureTestApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ComponentModel;

namespace AzureTestApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TableController : ControllerBase
    {
        private readonly TableServiceClient _tableServiceClient;
        private readonly TableClient _tableClient;

        public TableController(IOptions<AzureBlobModel> blobSettings)
        {
            // Initialize the Table Service client with the connection string
            _tableServiceClient = new TableServiceClient(blobSettings.Value.ConnectionString);

            // Create a client for the "DemoMessages" table
            _tableClient = _tableServiceClient.GetTableClient("DemoMessages");

            // Ensure the table exists
            _tableClient.CreateIfNotExists();
        }

        // POST: api/Table/add (Create an entity in the "DemoMessages" table)
        [HttpPost("add")]
        public async Task<IActionResult> AddEntity([FromBody] MessageTableModel message)
        {
            try
            {
                // Add a new entity to the table
                await _tableClient.AddEntityAsync(message);
                return Ok(new { message = "Message added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error adding entity to table.", details = ex.Message });
            }
        }

        // GET: api/Table/{partitionKey}/{rowKey} (Retrieve an entity from the "DemoMessages" table)
        [HttpGet("{partitionKey}/{rowKey}")]
        public async Task<IActionResult> GetEntity(
            [FromRoute, DefaultValue("DefaultPartition")] string partitionKey,
            [FromRoute, DefaultValue("rowkey-default")] string rowKey)
        {
            try
            {
                // Retrieve the entity using the PartitionKey and RowKey
                var message = await _tableClient.GetEntityAsync<MessageTableModel>(partitionKey, rowKey);
                return Ok(message.Value);
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                return NotFound(new { message = "Message not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error retrieving entity from table.", details = ex.Message });
            }
        }

        // PUT: api/Table/update (Update an existing entity in the "DemoMessages" table)
        [HttpPut("update")]
        public async Task<IActionResult> UpdateEntity([FromBody] MessageTableModel message)
        {
            try
            {
                // Update an existing entity in the table
                await _tableClient.UpdateEntityAsync(message, ETag.All, TableUpdateMode.Replace);
                return Ok(new { message = "Message updated successfully." });
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                return NotFound(new { message = "Message not found for update." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error updating entity in table.", details = ex.Message });
            }
        }

        // DELETE: api/Table/delete/{partitionKey}/{rowKey} (Delete an entity from the "DemoMessages" table)
        [HttpDelete("delete/{partitionKey}/{rowKey}")]
        public async Task<IActionResult> DeleteEntity(string partitionKey, string rowKey)
        {
            try
            {
                // Delete the entity from the table
                await _tableClient.DeleteEntityAsync(partitionKey, rowKey);
                return Ok(new { message = "Message deleted successfully." });
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                return NotFound(new { message = "Message not found for deletion." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error deleting entity from table.", details = ex.Message });
            }
        }
    }
}
