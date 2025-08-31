using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fullstack.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    
    public class HomeController : Controller
    {


        [Authorize]
        [HttpGet("secure")]
        public IActionResult SecureEndpoint()
        {
            return Ok("Token is valid");
        }
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Welcome to the Fullstack API!");
        }
    }
}
