using Microsoft.AspNetCore.Mvc;
using ML.SAL.DTO;
using ML.SAL.Interfaces;

namespace ML.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public IActionResult Register(UserDTO user)
        {
            _authService.Register(user);
            return Ok();
        }

        [HttpPost("login")]
        public IActionResult Login(UserDTO user)
        {
            var token = _authService.Login(user);
            if (token == null)
            {
                return Unauthorized();
            }
            return Ok(new { Token = token });
        }
        
        [HttpGet("search")]
        public async Task<IActionResult> SearchUsers([FromQuery] string query)
        {
            var users = await _authService.SearchUsers(query);
            return Ok(users);
        }
        
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            if (HttpContext.Items["UserId"] is int userId)
            {
                var user = await _authService.GetUserById(userId);
                if (user != null)
                {
                    return Ok(new { UserId = user.Id, Username = user.Username });
                }
            }

            return Unauthorized();
        }
    }
}