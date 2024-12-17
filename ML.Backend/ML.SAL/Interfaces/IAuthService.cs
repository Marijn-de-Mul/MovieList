using ML.SAL.DTO;

namespace ML.SAL.Interfaces;

public interface IAuthService
{
    void Register(UserDTO user);
    string Login(UserDTO user);
}