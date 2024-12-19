using ML.DAL.Data;
using ML.DAL.Interfaces;
using ML.SAL.DTO;
using ML.SAL.Interfaces;
using ML.SAL.Models;

namespace ML.DAL.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _context;

        public MovieRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(IMovie movie)
        {
            var movieEntity = new MovieDTO
            {
                Id = movie.Id,
                TheMovieDbId = movie.TheMovieDbId,
                Title = movie.Title,
                Description = movie.Description
            };
            _context.Movies.Add(movieEntity);
            _context.SaveChanges();
        }

        public IMovie GetById(int id)
        {
            return _context.Movies.Find(id);
        }

        public List<IMovie> Search(string query)
        {
            return _context.Movies
                .Where(m => m.Title.Contains(query))
                .Cast<IMovie>()
                .ToList();
        }

        public void Update(IMovie movie)
        {
            var movieEntity = new MovieDTO
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description
            };
            _context.Movies.Update(movieEntity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var movie = _context.Movies.Find(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                _context.SaveChanges();
            }
        }
    }
}