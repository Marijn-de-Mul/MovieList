using Microsoft.AspNetCore.Mvc;
using ML.API.DTO;

namespace ML.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieListController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateList(MovieListDTO list)
        {
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult EditList(int id, MovieListDTO list)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteList(int id)
        {
            return Ok();
        }

        [HttpPost("{id}/share")]
        public IActionResult ShareList(int id, UserDTO user)
        {
            return Ok();
        }

        [HttpDelete("{id}/share/{userId}")]
        public IActionResult RemoveUserFromList(int id, int userId)
        {
            return Ok();
        }
    }
}