using ML.DAL.Data;
using ML.DAL.Interfaces;

namespace ML.DAL.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }
}