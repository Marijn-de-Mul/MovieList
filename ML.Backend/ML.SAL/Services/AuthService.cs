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
                return "generated_token";
            }
            return null;
        }
    }
}