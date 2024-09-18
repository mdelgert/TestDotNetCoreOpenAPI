using Microsoft.AspNetCore.Mvc;

namespace AzureTestApp.Controllers
{
    public class BlobController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
