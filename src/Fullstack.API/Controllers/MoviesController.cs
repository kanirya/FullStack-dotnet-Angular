using Fullstack.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fullstack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly MovieService _movieService;
        public MoviesController(MovieService movieService)
        {
            _movieService = movieService;

        }
        [HttpGet]
        public async Task<IActionResult> GetMovies([FromQuery] string? query)
        {
            var movies = await _movieService.SearchMoviesAsync(query);
            return Ok(movies);
        }
    }
}
