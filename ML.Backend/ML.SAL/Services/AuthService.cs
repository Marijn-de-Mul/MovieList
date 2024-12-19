using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ML.SAL.DTO;
using ML.SAL.Interfaces;
using ML.DAL.Interfaces;

namespace ML.SAL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void Register(UserDTO user)
        {
            var userEntity = new UserDTO
            {
                Username = user.Username,
                Password = user.Password 
            };
            _userRepository.Add(userEntity);
        }

        public string Login(UserDTO user)
        {
            var userEntity = _userRepository.GetByUsername(user.Username);
            if (userEntity != null && userEntity.Password == user.Password)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET"));
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim("id", userEntity.Id.ToString()) }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            return null;
        }
        
        public async Task<List<UserDTO>> SearchUsers(string query)
        {
            var users = _userRepository.Search(query)
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    Username = u.Username
                }).ToList();

            return await Task.FromResult(users);
        }
        
        public async Task<UserDTO> GetUserById(int userId)
        {
            var user = await _userRepository.GetById(userId);
            if (user == null)
            {
                return null;
            }

            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
            };
        }
    }
}