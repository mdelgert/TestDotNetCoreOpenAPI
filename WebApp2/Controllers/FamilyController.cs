using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace WebApp2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FamilyController : ControllerBase
    {
        private readonly ILogger<FamilyController> _logger;
        private readonly CosmosDbService _cosmosDbService;

        public FamilyController(ILogger<FamilyController> logger, CosmosDbService cosmosDbService)
        {
            _logger = logger;
            _cosmosDbService = cosmosDbService;
        }

        [HttpGet("GetFamilyById")]
        public async Task<ActionResult<Family>> GetFamilyById([FromQuery] 
        string id = "Test.1", [FromQuery] 
        string partitionKey = "Test")
        {
            var family = await _cosmosDbService.GetFamilyByIdAsync(id, partitionKey);
            if (family == null)
            {
                return NotFound();
            }
            return Ok(family);
        }

        // New POST method
        [HttpPost]
        public async Task<ActionResult<Family>> CreateFamily([FromBody] 
        Family family, [FromQuery] 
        string id = "Test.1", [FromQuery] 
        string partitionKey = "Test")
        {
            if (family == null)
            {
                return BadRequest("Family data is required.");
            }

            // Set default values if not provided
            family.Id = id;
            family.PartitionKey = partitionKey;

            var createdFamily = await _cosmosDbService.CreateFamilyAsync(family);
            return CreatedAtAction(nameof(GetFamilyById), new 
            { id = createdFamily.Id, partitionKey = createdFamily.PartitionKey }, createdFamily);
        }

    }
}

