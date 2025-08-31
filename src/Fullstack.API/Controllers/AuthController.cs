using Fullstack.Application.DTOs;
using Fullstack.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fullstack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        public AuthController(IAuthService auth) { _auth = auth; }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try { var res = await _auth.RegisterAsync(dto, HttpContext.Connection.RemoteIpAddress?.ToString()); return Ok(res); }
            catch (Exception ex) { return BadRequest(new { error = ex.Message }); }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try { var res = await _auth.LoginAsync(dto, HttpContext.Connection.RemoteIpAddress?.ToString()); return Ok(res); }
            catch (Exception ex) { return Unauthorized(new { error = ex.Message }); }
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequestDto dto)
        {
            try { var res = await _auth.RefreshAsync(dto.RefreshToken, HttpContext.Connection.RemoteIpAddress?.ToString()); return Ok(res); }
            catch (Exception ex) { return Unauthorized(new { error = ex.Message }); }
        }


        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke([FromBody] RefreshRequestDto dto)
        {
            await _auth.RevokeAsync(dto.RefreshToken, HttpContext.Connection.RemoteIpAddress?.ToString());
            return Ok(new { success = true });
        }
    }
}
