using Microsoft.AspNetCore.Mvc;
using WebApp3;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        // POST: api/echo
        [HttpPost]
        public IActionResult Post([FromBody] MessageModel model)
        {
            // Return the received message as a JSON response
            return Ok(new { message = model.Message });
        }

        // Optionally, for GET request support, you could use query parameters
        // GET: api/echo?message=hello%20world
        [HttpGet]
        public IActionResult Get([FromQuery] string message)
        {
            // Return the received message as a JSON response
            return Ok(new { message });
        }
    }
}
