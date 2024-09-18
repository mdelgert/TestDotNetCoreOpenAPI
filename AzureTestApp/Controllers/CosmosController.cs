using AzureTestApp.Models;
using AzureTestApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureTestApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CosmosController : ControllerBase
    {
        private readonly ILogger<CosmosController> _logger;
        private readonly CosmosDbService _cosmosDbService;

        public CosmosController(ILogger<CosmosController> logger, CosmosDbService cosmosDbService)
        {
            _logger = logger;
            _cosmosDbService = cosmosDbService;
        }

        [HttpGet("GetNoteById")]
        public async Task<ActionResult<NoteCosmosModel>> GetNoteById([FromQuery]
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
        public async Task<ActionResult<NoteCosmosModel>> CreateNote([FromBody]
        NoteCosmosModel note, [FromQuery]
        string id = "Note.1", [FromQuery]
        string partitionKey = "Note1",
        string message = "HelloCosmos!")
        {
            // Set default values if not provided
            note.Id = id;
            note.PartitionKey = partitionKey;
            note.Message = message;

            var createdNote = await _cosmosDbService.CreateNoteAsync(note);
            return CreatedAtAction(nameof(GetNoteById), new
            { id = createdNote.Id, partitionKey = createdNote.PartitionKey }, createdNote);
        }
    }
}
