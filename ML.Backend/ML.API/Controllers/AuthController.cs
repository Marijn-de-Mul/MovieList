using Microsoft.AspNetCore.Mvc;
using ML.API.DTO;

namespace ML.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("register")]
        public IActionResult Register(UserDTO user)
        {
            return Ok();
        }

        [HttpPost("login")]
        public IActionResult Login(UserDTO user)
        {
            // Login logic
            return Ok();
        }
    }
}