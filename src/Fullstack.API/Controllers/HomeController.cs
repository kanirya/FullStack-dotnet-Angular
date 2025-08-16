using Microsoft.AspNetCore.Mvc;

namespace Fullstack.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Welcome to the Fullstack API!");
        }
    }
}
