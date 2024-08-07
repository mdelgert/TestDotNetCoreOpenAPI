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
        public async Task<ActionResult<Note>> GetNoteById([FromQuery] 
        string id = "Note.1", [FromQuery] 
        string partitionKey = "Note1")
        {
            var note = await _cosmosDbService.GetNoteByIdAsync(id, partitionKey);
            if (note == null)
            {
                return NotFound();
            }
            return Ok(note);
        }
        
        [HttpPost]
        public async Task<ActionResult<Note>> CreateNote([FromBody] 
        Note note, [FromQuery] 
        string id = "Note.1", [FromQuery] 
        string partitionKey = "Note1",
        string message = "Hello World")
        {
            // Set default values if not provided
            note.Id = id;
            note.PartitionKey = partitionKey;
            note.Message = message;

            var createdNote = await _cosmosDbService.CreateNoteAsync(note);
            return CreatedAtAction(nameof(GetNoteById), new 
            { id = createdNote.Id, partitionKey = createdNote.PartitionKey }, createdNote);
        }

        [HttpDelete("DeleteNote")]
        public async Task<IActionResult> DeleteNote([FromQuery] 
        string id = "Note.1", [FromQuery] 
        string partitionKey = "Note1")
        {
            try
            {
                await _cosmosDbService.DeleteNoteAsync(id, partitionKey);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting note");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

