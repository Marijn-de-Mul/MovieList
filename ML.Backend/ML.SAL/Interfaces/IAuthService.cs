using ML.SAL.DTO;

namespace ML.SAL.Interfaces;

public interface IAuthService
{
    void Register(UserDTO user);
    string Login(UserDTO user);
    Task<List<UserDTO>> SearchUsers(string query);
    Task<UserDTO> GetUserById(int userId);
}