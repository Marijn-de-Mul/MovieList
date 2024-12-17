using ML.SAL.Interfaces;

namespace ML.SAL.DTO;

public class UserDTO : IUser
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}