using ML.DAL.Data;
using ML.DAL.Interfaces;
using ML.SAL.DTO;
using ML.SAL.Interfaces;
using ML.SAL.Models;

namespace ML.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(IUser user)
        {
            var userEntity = new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Password = user.Password
            };
            _context.Users.Add(userEntity);
            _context.SaveChanges();
        }

        public IUser GetByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }
        
        public IEnumerable<IUser> Search(string query)
        {
            return _context.Users
                .Where(u => u.Username.Contains(query))
                .ToList();
        }
        
        public async Task<UserDTO> GetById(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return null;
            }

            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Password = user.Password
            };
        }
    }
}