using Microsoft.AspNetCore.Mvc;
using ML.SAL.DTO;
using ML.SAL.Interfaces;

namespace ML.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieListController : ControllerBase
    {
        private readonly IMovieListService _movieListService;

        public MovieListController(IMovieListService movieListService)
        {
            _movieListService = movieListService;
        }

        [HttpGet]
        public IActionResult GetListsByUser()
        {
            if (HttpContext.Items["UserId"] is int userId)
            {
                var lists = _movieListService.GetListsByUser(userId);
                return Ok(lists);
            }

            return Unauthorized();
        }

        [HttpPost]
        public IActionResult CreateList([FromBody] MovieListDTO list)
        {
            if (HttpContext.Items["UserId"] is int userId)
            {
                list.userId = userId;
                
                _movieListService.CreateList(list);
                return Ok();
            }

            return Unauthorized();
        }

        [HttpPut("{id}")]
        public IActionResult EditList(int id, [FromBody] MovieListDTO list)
        {
            _movieListService.EditList(id, list);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteList(int id)
        {
            _movieListService.DeleteList(id);
            return Ok();
        }

        [HttpPost("{id}/share")]
        public IActionResult ShareList(int id, [FromBody] UserDTO user)
        {
            _movieListService.ShareList(id, user);
            return Ok();
        }

        [HttpDelete("{id}/user/{userId}")]
        public IActionResult RemoveUserFromList(int id, int userId)
        {
            _movieListService.RemoveUserFromList(id, userId);
            return Ok();
        }
    }
}