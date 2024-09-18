using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AzureTestApp.Models;
using AzureTestApp.Services;

namespace AzureTestApp.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class SqlController : ControllerBase
    {
        private readonly NotesDbService _context;
        private readonly ILogger<SqlController> _logger;

        public SqlController(NotesDbService context, ILogger<SqlController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: [controller]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NoteSqlModel>>> Get()
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
