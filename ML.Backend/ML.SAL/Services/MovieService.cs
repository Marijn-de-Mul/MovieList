using System.Diagnostics;
using System.Globalization;
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
                Description = movie.Description,
                BannerUrl = movie.BannerUrl
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
                Description = movie.Description,
                BannerUrl = movie.BannerUrl
            };
        }

        public async Task<List<MovieDTO>> SearchMovies(string query)
        {
            var movies = _movieRepository.Search(query).Select(m => new MovieDTO
            {
                Id = m.Id,
                TheMovieDbId = m.TheMovieDbId,
                Title = m.Title,
                Description = m.Description,
                BannerUrl = m.BannerUrl 
            }).ToList();

            var uniqueMovies =
                new HashSet<(string Title, string Description)>(movies.Select(m => (m.Title, m.Description)));

            if (movies.Count < 10)
            {
                Console.WriteLine("Fetching movies and TV shows from TMDB API...");

                var client = new RestClient("https://api.themoviedb.org/3/");
                int maxPages = 1000;
                string region = "US";

                int page = 1;
                bool morePages = true;
                while (morePages && page <= maxPages)
                {
                    var movieRequest = new RestRequest("search/movie", Method.Get);
                    movieRequest.AddHeader("Authorization", $"Bearer {_tmdbApiKey}");
                    movieRequest.AddParameter("query", query);
                    movieRequest.AddParameter("region", region);
                    movieRequest.AddParameter("sort_by", "popularity.desc");
                    movieRequest.AddParameter("page", page);

                    var movieResponse = await client.ExecuteAsync(movieRequest);

                    if (movieResponse.IsSuccessful)
                    {
                        var movieContent = JObject.Parse(movieResponse.Content);

                        foreach (var result in movieContent["results"])
                        {
                            string title = (string)result["title"];
                            string description = (string)result["overview"];
                            string bannerUrl = result["poster_path"] != null
                                ? $"https://image.tmdb.org/t/p/w500{result["poster_path"]}"
                                : null;

                            if (!uniqueMovies.Contains((title, description)))
                            {
                                var movie = new MovieDTO
                                {
                                    Id = 0,
                                    TheMovieDbId = (int)result["id"],
                                    Title = title,
                                    Description = description,
                                    BannerUrl = bannerUrl
                                };

                                _movieRepository.Add(movie);
                                uniqueMovies.Add((title, description));

                                var addedMovie = _movieRepository.Search(movie.Title).FirstOrDefault();
                                if (addedMovie != null)
                                {
                                    movies.Add(new MovieDTO
                                    {
                                        Id = addedMovie.Id,
                                        TheMovieDbId = addedMovie.TheMovieDbId,
                                        Title = addedMovie.Title,
                                        Description = addedMovie.Description,
                                        BannerUrl = string.IsNullOrEmpty(addedMovie.BannerUrl) || addedMovie.BannerUrl == "https://image.tmdb.org/t/p/w500"
                                            ? "https://via.placeholder.com/200x300"
                                            : addedMovie.BannerUrl
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

                page = 1;
                morePages = true;
                while (morePages && page <= maxPages)
                {
                    var tvRequest = new RestRequest("search/tv", Method.Get);
                    tvRequest.AddHeader("Authorization", $"Bearer {_tmdbApiKey}");
                    tvRequest.AddParameter("query", query);
                    tvRequest.AddParameter("region", region);
                    tvRequest.AddParameter("sort_by", "popularity.desc");
                    tvRequest.AddParameter("page", page);

                    var tvResponse = await client.ExecuteAsync(tvRequest);

                    if (tvResponse.IsSuccessful)
                    {
                        var tvContent = JObject.Parse(tvResponse.Content);

                        foreach (var result in tvContent["results"])
                        {
                            string title = (string)result["name"];
                            string description = (string)result["overview"];
                            string bannerUrl = result["poster_path"] != null
                                ? $"https://image.tmdb.org/t/p/w500{result["poster_path"]}"
                                : null;

                            if (!uniqueMovies.Contains((title, description)))
                            {
                                var tvShow = new MovieDTO
                                {
                                    Id = 0,
                                    TheMovieDbId = (int)result["id"],
                                    Title = title,
                                    Description = description,
                                    BannerUrl = bannerUrl
                                };

                                _movieRepository.Add(tvShow);
                                uniqueMovies.Add((title, description));

                                var addedTvShow = _movieRepository.Search(tvShow.Title).FirstOrDefault();
                                if (addedTvShow != null)
                                {
                                    movies.Add(new MovieDTO
                                    {
                                        Id = addedTvShow.Id,
                                        TheMovieDbId = addedTvShow.TheMovieDbId,
                                        Title = addedTvShow.Title,
                                        Description = addedTvShow.Description,
                                        BannerUrl = string.IsNullOrEmpty(addedTvShow.BannerUrl) || addedTvShow.BannerUrl == "https://image.tmdb.org/t/p/w500"
                                            ? "https://via.placeholder.com/200x300"
                                            : addedTvShow.BannerUrl
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
                Description = movie.Description,
                BannerUrl = movie.BannerUrl
            };
            _movieRepository.Update(movieEntity);
        }

        public void DeleteMovie(int id)
        {
            _movieRepository.Delete(id);
        }
    }
}