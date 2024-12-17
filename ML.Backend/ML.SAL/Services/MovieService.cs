using System.Diagnostics;
using RestSharp;
using Newtonsoft.Json.Linq;
using ML.DAL.Interfaces;
using ML.SAL.DTO;
using ML.SAL.Interfaces;

namespace ML.SAL.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly string _tmdbApiKey = Environment.GetEnvironmentVariable("TMDB_API_KEY");
            
        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public void AddMovie(MovieDTO movie)
        {
            IMovie movieEntity = new MovieDTO
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description
            };
            _movieRepository.Add(movieEntity);
        }

        public MovieDTO GetMovieById(int id)
        {
            var movie = _movieRepository.GetById(id);
            return new MovieDTO
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description
            };
        }

        public async Task<List<MovieDTO>> SearchMovies(string query)
        {
            var movies = _movieRepository.Search(query).Select(m => new MovieDTO
            {
                Id = m.Id,
                TheMovieDbId = m.TheMovieDbId,
                Title = m.Title,
                Description = m.Description
            }).ToList();

            if (movies.Count < 10)
            {
                Console.WriteLine("Fetching movies and TV shows from TMDB API...");

                var client = new RestClient("https://api.themoviedb.org/3/");
                int maxPages = 5; // Set a maximum page limit to avoid infinite loops

                // Fetch all pages of movies
                int page = 1;
                bool morePages = true;
                while (morePages && page <= maxPages)
                {
                    var movieRequest = new RestRequest("discover/movie", Method.Get);
                    movieRequest.AddHeader("Authorization", $"Bearer {_tmdbApiKey}");
                    movieRequest.AddParameter("language", "en-US");
                    movieRequest.AddParameter("sort_by", "popularity.desc");
                    movieRequest.AddParameter("page", page);

                    var movieResponse = await client.ExecuteAsync(movieRequest);

                    if (movieResponse.IsSuccessful)
                    {
                        var movieContent = JObject.Parse(movieResponse.Content);

                        foreach (var result in movieContent["results"])
                        {
                            int tmdbId = (int)result["id"];
                            if (!_movieRepository.Search(tmdbId.ToString()).Any())
                            {
                                var movie = new MovieDTO
                                {
                                    Id = 0,
                                    TheMovieDbId = tmdbId,
                                    Title = (string)result["title"],
                                    Description = (string)result["overview"]
                                };

                                _movieRepository.Add(movie);

                                var addedMovie = _movieRepository.Search(movie.Title).FirstOrDefault();
                                if (addedMovie != null)
                                {
                                    movies.Add(new MovieDTO
                                    {
                                        Id = addedMovie.Id,
                                        TheMovieDbId = addedMovie.TheMovieDbId,
                                        Title = addedMovie.Title,
                                        Description = addedMovie.Description
                                    });
                                }
                            }
                        }

                        morePages = page < (int)movieContent["total_pages"];
                        page++;
                    }
                    else
                    {
                        Console.WriteLine("Failed to fetch movies from TMDB API.");
                        Console.WriteLine("Status Code: " + movieResponse.StatusCode);
                        Console.WriteLine("Error Message: " + movieResponse.ErrorMessage);
                        Console.WriteLine("Content: " + movieResponse.Content);
                        morePages = false;
                    }
                }

                // Fetch all pages of TV shows
                page = 1;
                morePages = true;
                while (morePages && page <= maxPages)
                {
                    var tvRequest = new RestRequest("discover/tv", Method.Get);
                    tvRequest.AddHeader("Authorization", $"Bearer {_tmdbApiKey}");
                    tvRequest.AddParameter("language", "en-US");
                    tvRequest.AddParameter("sort_by", "popularity.desc");
                    tvRequest.AddParameter("page", page);

                    var tvResponse = await client.ExecuteAsync(tvRequest);

                    if (tvResponse.IsSuccessful)
                    {
                        var tvContent = JObject.Parse(tvResponse.Content);

                        foreach (var result in tvContent["results"])
                        {
                            int tmdbId = (int)result["id"];
                            if (!_movieRepository.Search(tmdbId.ToString()).Any())
                            {
                                var tvShow = new MovieDTO
                                {
                                    Id = 0,
                                    TheMovieDbId = tmdbId,
                                    Title = (string)result["name"],
                                    Description = (string)result["overview"]
                                };

                                _movieRepository.Add(tvShow);

                                var addedTvShow = _movieRepository.Search(tvShow.Title).FirstOrDefault();
                                if (addedTvShow != null)
                                {
                                    movies.Add(new MovieDTO
                                    {
                                        Id = addedTvShow.Id,
                                        TheMovieDbId = addedTvShow.TheMovieDbId,
                                        Title = addedTvShow.Title,
                                        Description = addedTvShow.Description
                                    });
                                }
                            }
                        }

                        morePages = page < (int)tvContent["total_pages"];
                        page++;
                    }
                    else
                    {
                        Console.WriteLine("Failed to fetch TV shows from TMDB API.");
                        Console.WriteLine("Status Code: " + tvResponse.StatusCode);
                        Console.WriteLine("Error Message: " + tvResponse.ErrorMessage);
                        Console.WriteLine("Content: " + tvResponse.Content);
                        morePages = false;
                    }
                }
            }

            return movies;
        }

        public void UpdateMovie(MovieDTO movie)
        {
            IMovie movieEntity = new MovieDTO
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description
            };
            _movieRepository.Update(movieEntity);
        }

        public void DeleteMovie(int id)
        {
            _movieRepository.Delete(id);
        }
    }
}