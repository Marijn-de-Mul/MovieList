using Microsoft.AspNetCore.Mvc;
using ML.SAL.Interfaces;

namespace ML.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchMovies(string query)
        {
            var movies = await _movieService.SearchMovies(query);
            return Ok(movies);
        }
        
        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetMovieDetails(int id)
        {
            var movie = _movieService.GetMovieById(id);
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(movie);
        }
    }
}