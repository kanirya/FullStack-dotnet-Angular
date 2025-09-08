using Fullstack.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fullstack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        public readonly IUserDataRepository _userRepo;
        public ProfileController(IUserDataRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserData(Guid id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null) return Ok("User Not Found");

            return Ok(user);
        }
    }
}
