using Microsoft.EntityFrameworkCore;
using ML.DAL.Data;
using ML.SAL.Interfaces;
using ML.SAL.Models;
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
            var userLists = _context.MovieLists
                .Where(list => list.userId == userId)
                .Include(list => list.Movies)
                .ThenInclude(mlm => mlm.Movie)
                .Include(list => list.SharedWith)
                .ThenInclude(mlsw => mlsw.User)
                .Select(list => new MovieListDTO
                {
                    Id = list.Id,
                    Name = list.Name,
                    userId = list.userId,
                    Movies = list.Movies.Select(mlm => new MovieListMoviesDTO
                    {
                        MovieListId = list.Id,
                        MovieId = mlm.Movie.Id,
                        Movie = new MovieDTO
                        {
                            Id = mlm.Movie.Id,
                            Title = mlm.Movie.Title,
                            Description = mlm.Movie.Description
                        }
                    }).ToList(),
                    SharedWith = list.SharedWith.Select(mlsw => new MovieListSharedWithDTO
                    {
                        MovieListId = list.Id,
                        UserId = mlsw.User.Id,
                        User = new UserDTO
                        {
                            Id = mlsw.User.Id,
                            Username = mlsw.User.Username
                        }
                    }).ToList()
                }).ToList<IMovieList>();

            var sharedLists = _context.MovieLists
                .Where(list => list.SharedWith.Any(mlsw => mlsw.UserId == userId))
                .Include(list => list.Movies)
                .ThenInclude(mlm => mlm.Movie)
                .Include(list => list.SharedWith)
                .ThenInclude(mlsw => mlsw.User)
                .Select(list => new MovieListDTO
                {
                    Id = list.Id,
                    Name = list.Name,
                    userId = list.userId,
                    Movies = list.Movies.Select(mlm => new MovieListMoviesDTO
                    {
                        MovieListId = list.Id,
                        MovieId = mlm.Movie.Id,
                        Movie = new MovieDTO
                        {
                            Id = mlm.Movie.Id,
                            Title = mlm.Movie.Title,
                            Description = mlm.Movie.Description
                        }
                    }).ToList(),
                    SharedWith = list.SharedWith.Select(mlsw => new MovieListSharedWithDTO
                    {
                        MovieListId = list.Id,
                        UserId = mlsw.User.Id,
                        User = new UserDTO
                        {
                            Id = mlsw.User.Id,
                            Username = mlsw.User.Username
                        }
                    }).ToList()
                }).ToList<IMovieList>();

            return userLists.Concat(sharedLists).ToList();
        }

        public void CreateList(IMovieList list)
        {
            var movieList = new MovieListDTO
            {
                Name = list.Name,
                userId = list.userId
            };

            _context.MovieLists.Add(movieList);
            _context.SaveChanges();

            foreach (var movie in list.Movies)
            {
                var movieListMovie = new MovieListMoviesDTO
                {
                    MovieListId = movieList.Id,
                    MovieId = movie.MovieId
                };
                _context.MovieListMovies.Add(movieListMovie);
            }

            foreach (var user in list.SharedWith)
            {
                var movieListSharedWith = new MovieListSharedWithDTO
                {
                    MovieListId = movieList.Id,
                    UserId = user.UserId
                };
                _context.MovieListSharedWith.Add(movieListSharedWith);
            }

            _context.SaveChanges();
        }

        public void EditList(int id, IMovieList list)
        {
            var existingList = _context.MovieLists
                .Include(ml => ml.Movies)
                .Include(ml => ml.SharedWith)
                .FirstOrDefault(ml => ml.Id == id);

            if (existingList != null)
            {
                existingList.Name = list.Name;

                _context.MovieListMovies.RemoveRange(_context.MovieListMovies.Where(mlm => mlm.MovieListId == existingList.Id));
                foreach (var movie in list.Movies)
                {
                    var movieListMovie = new MovieListMoviesDTO
                    {
                        MovieListId = existingList.Id,
                        MovieId = movie.MovieId
                    };
                    _context.MovieListMovies.Add(movieListMovie);
                }

                _context.MovieListSharedWith.RemoveRange(_context.MovieListSharedWith.Where(mlsw => mlsw.MovieListId == existingList.Id));
                foreach (var user in list.SharedWith)
                {
                    var movieListSharedWith = new MovieListSharedWithDTO
                    {
                        MovieListId = existingList.Id,
                        UserId = user.UserId
                    };
                    _context.MovieListSharedWith.Add(movieListSharedWith);
                }

                _context.SaveChanges();
            }
        }

        public void DeleteList(int id)
        {
            var movieList = _context.MovieLists
                .Include(ml => ml.Movies)
                .Include(ml => ml.SharedWith)
                .FirstOrDefault(ml => ml.Id == id);

            if (movieList != null)
            {
                _context.MovieListMovies.RemoveRange(movieList.Movies);
                _context.MovieListSharedWith.RemoveRange(movieList.SharedWith);
                _context.MovieLists.Remove(movieList);
                _context.SaveChanges();
            }
        }

        public void ShareList(int id, IUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var movieList = _context.MovieLists.Include(ml => ml.SharedWith).FirstOrDefault(ml => ml.Id == id);
            if (movieList == null)
            {
                throw new InvalidOperationException("Movie list not found.");
            }

            var userEntity = _context.Users.FirstOrDefault(u => u.Username == user.Username);
            if (userEntity == null)
            {
                throw new InvalidOperationException($"User not found. Username: {user.Username}");
            }

            _context.Attach(userEntity);

            movieList.SharedWith.Add(new MovieListSharedWithDTO
            {
                MovieListId = id,
                UserId = userEntity.Id
            });

            _context.SaveChanges();
        }

        public void RemoveUserFromList(int id, int userId)
        {
            var movieList = _context.MovieLists
                .Include(ml => ml.SharedWith)
                .FirstOrDefault(ml => ml.Id == id);

            if (movieList != null && movieList.SharedWith != null)
            {
                var userToRemove = movieList.SharedWith.FirstOrDefault(sw => sw.UserId == userId);
                if (userToRemove != null)
                {
                    _context.MovieListSharedWith.Remove(userToRemove);
                    _context.SaveChanges();
                }
            }
        }
        
        public void RemoveMovieFromList(int listId, int movieId)
        {
            var movieListMovie = _context.MovieListMovies
                .FirstOrDefault(mlm => mlm.MovieListId == listId && mlm.MovieId == movieId);

            if (movieListMovie != null)
            {
                _context.MovieListMovies.Remove(movieListMovie);
                _context.SaveChanges();
            }
        }
    }
}