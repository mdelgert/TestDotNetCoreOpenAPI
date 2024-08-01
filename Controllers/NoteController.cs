using WebApp1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace WebApp1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly NotesDbContext _context;
        private readonly ILogger<NoteController> _logger;

        public NoteController(NotesDbContext context, ILogger<NoteController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: [controller]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NoteModel>>> Get()
        {
            try
            {
                var notes = await _context.Notes.ToListAsync();
                return Ok(notes);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving data from the database.");
                return StatusCode(500, "A database error occurred.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving data.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
