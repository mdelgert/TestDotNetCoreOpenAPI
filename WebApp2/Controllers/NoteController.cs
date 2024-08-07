using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace WebApp2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly ILogger<NoteController> _logger;
        private readonly CosmosDbService _cosmosDbService;

        public NoteController(ILogger<NoteController> logger, CosmosDbService cosmosDbService)
        {
            _logger = logger;
            _cosmosDbService = cosmosDbService;
        }

        [HttpGet("GetNoteById")]
        public async Task<ActionResult<Note>> GetFamilyById([FromQuery] 
        string id = "Note.1", [FromQuery] 
        string partitionKey = "Note1")
        {
            var note = await _cosmosDbService.GetFamilyByIdAsync(id, partitionKey);
            if (note == null)
            {
                return NotFound();
            }
            return Ok(note);
        }

        // New POST method
        [HttpPost]
        public async Task<ActionResult<Note>> CreateNote([FromBody] 
        Note note, [FromQuery] 
        string id = "Note.1", [FromQuery] 
        string partitionKey = "Note1")
        {
            if (note == null)
            {
                return BadRequest("Note data is required.");
            }

            // Set default values if not provided
            note.Id = id;
            note.PartitionKey = partitionKey;

            var createdFamily = await _cosmosDbService.CreateFamilyAsync(note);
            return CreatedAtAction(nameof(GetFamilyById), new 
            { id = createdFamily.Id, partitionKey = createdFamily.PartitionKey }, createdFamily);
        }

    }
}

