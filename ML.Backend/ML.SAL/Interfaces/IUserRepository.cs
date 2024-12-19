using ML.SAL.DTO;
using ML.SAL.Interfaces;

namespace ML.DAL.Interfaces;

public interface IUserRepository
{
    void Add(IUser user);
    IUser GetByUsername(string username);
    Task<UserDTO> GetById(int userId);
    IEnumerable<IUser> Search(string query);
}