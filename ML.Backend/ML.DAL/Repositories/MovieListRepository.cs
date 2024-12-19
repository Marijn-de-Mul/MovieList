using Microsoft.EntityFrameworkCore;
using ML.DAL.Data;
using ML.SAL.Interfaces;
using ML.SAL.Models;
using System.Linq;
using ML.DAL.Interfaces;
using ML.SAL.DTO;

namespace ML.DAL.Repositories
{
    public class MovieListRepository : IMovieListRepository
    {
        private readonly ApplicationDbContext _context;

        public MovieListRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public List<IMovieList> GetListsByUser(int userId)
        {
            return _context.MovieLists.Where(list => list.userId == userId)
                .Select(list => new MovieListDTO
                {
                    Id = list.Id,
                    Name = list.Name,
                    userId = list.userId,
                    Movies = list.Movies.Select(m => new MovieDTO
                    {
                        Id = m.Id,
                        Title = m.Title,
                        Description = m.Description
                    }).ToList()
                }).ToList<IMovieList>();
        }

        public void CreateList(IMovieList list)
        {
            var movieList = new MovieList
            {
                Name = list.Name,
                userId = list.userId, 
                Movies = list.Movies.Select(m => new MovieDTO
                {
                    Id = m.Id,
                    Title = m.Title,
                    Description = m.Description
                }).ToList()
            };
            _context.MovieLists.Add(movieList);
            _context.SaveChanges();
        }

        public void EditList(int id, IMovieList list)
        {
            var existingList = _context.MovieLists.Find(id);
            if (existingList != null)
            {
                existingList.Name = list.Name;
                existingList.Movies = list.Movies.Select(m => new MovieDTO
                {
                    Id = m.Id,
                    Title = m.Title,
                    Description = m.Description
                }).ToList();
                _context.SaveChanges();
            }
        }

        public void DeleteList(int id)
        {
            var list = _context.MovieLists.Find(id);
            if (list != null)
            {
                _context.MovieLists.Remove(list);
                _context.SaveChanges();
            }
        }

        public void ShareList(int id, IUser user)
        {
            var list = _context.MovieLists.Find(id);
            if (list != null)
            {
                var userEntity = new UserDTO
                {
                    Id = user.Id,
                    Username = user.Username
                };
                list.SharedWith.Add(userEntity);
                _context.SaveChanges();
            }
        }

        public void RemoveUserFromList(int id, int userId)
        {
            var list = _context.MovieLists.Find(id);
            if (list != null)
            {
                var user = list.SharedWith.FirstOrDefault(u => u.Id == userId);
                if (user != null)
                {
                    list.SharedWith.Remove(user);
                    _context.SaveChanges();
                }
            }
        }
    }
}