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
    }
}