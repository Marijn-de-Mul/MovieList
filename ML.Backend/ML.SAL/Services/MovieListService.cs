﻿using ML.SAL.DTO;
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
                    userId = list.userId,
                    Movies = list.Movies.Select(mlm => new MovieListMoviesDTO
                    {
                        MovieListId = mlm.MovieListId,
                        MovieId = mlm.MovieId,
                        Movie = new MovieDTO
                        {
                            Id = mlm.Movie.Id,
                            Title = mlm.Movie.Title,
                            Description = mlm.Movie.Description,
                            BannerUrl = mlm.Movie.BannerUrl 
                        }
                    }).ToList(),
                    SharedWith = list.SharedWith.Select(mlsw => new MovieListSharedWithDTO
                    {
                        MovieListId = mlsw.MovieListId,
                        UserId = mlsw.UserId,
                        User = new UserDTO
                        {
                            Id = mlsw.User.Id,
                            Username = mlsw.User.Username
                        }
                    }).ToList()
                }).ToList();
        }

        public void CreateList(MovieListDTO list)
        {
            _movieListRepository.CreateList(list);
        }


        public void EditList(int id, MovieListDTO list, int userId)
        {
            var existingList = _movieListRepository.GetListsByUser(userId)
                .FirstOrDefault(l => l.Id == id);

            if (existingList == null)
            {
                throw new InvalidOperationException("Movie list not found.");
            }

            if (existingList.userId == userId)
            {
                _movieListRepository.EditList(id, list);
            }
            else if (existingList.SharedWith.Any(u => u.UserId == userId))
            {
                var sharedUser = existingList.SharedWith.First(u => u.UserId == userId);

                existingList.Movies = list.Movies;

                if (!list.SharedWith.Any(u => u.UserId == userId))
                {
                    existingList.SharedWith.Remove(sharedUser);
                }

                _movieListRepository.EditList(id, existingList);
            }
            else
            {
                throw new UnauthorizedAccessException("User does not have permission to edit this list.");
            }
        }

        public void DeleteList(int id)
        {
            _movieListRepository.DeleteList(id);
        }

        public void ShareList(int id, UserDTO user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var userEntity = new UserDTO
            {
                Id = user.Id,
                Username = user.Username
            };

            _movieListRepository.ShareList(id, userEntity);
        }

        public void RemoveUserFromList(int id, int userId)
        {
            _movieListRepository.RemoveUserFromList(id, userId);
        }
        
        public void RemoveMovieFromList(int listId, int movieId)
        {
            _movieListRepository.RemoveMovieFromList(listId, movieId);
        }
    }
}