using ML.SAL.Interfaces;

namespace ML.SAL.Models;

public class User : IUser
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public string Password { get; set; }
}