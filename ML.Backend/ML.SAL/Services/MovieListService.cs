using ML.SAL.DTO;
using ML.SAL.Interfaces;
using ML.DAL.Interfaces;
using System.Linq;

namespace ML.SAL.Services
{
    public class MovieListService : IMovieListService
    {
        private readonly IMovieListRepository _movieListRepository;

        public MovieListService(IMovieListRepository movieListRepository)
        {
            _movieListRepository = movieListRepository;
        }
        
        public List<MovieListDTO> GetListsByUser(int userId)
        {
            return _movieListRepository.GetListsByUser(userId)
                .Select(list => new MovieListDTO
                {
                    Id = list.Id,
                    Name = list.Name,
                    userId = list.userId
                }).ToList();
        }

        public void CreateList(MovieListDTO list)
        {
            _movieListRepository.CreateList(list);
        }

        public void EditList(int id, MovieListDTO list)
        {
            _movieListRepository.EditList(id, list);
        }

        public void DeleteList(int id)
        {
            _movieListRepository.DeleteList(id);
        }

        public void ShareList(int id, UserDTO user)
        {
            _movieListRepository.ShareList(id, user);
        }

        public void RemoveUserFromList(int id, int userId)
        {
            _movieListRepository.RemoveUserFromList(id, userId);
        }
    }
}