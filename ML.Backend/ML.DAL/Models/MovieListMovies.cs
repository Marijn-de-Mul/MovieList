using ML.SAL.DTO;
using ML.SAL.Interfaces;

namespace ML.SAL.Models;

public class MovieListMovies : IMovieListMovies
{
    public int MovieListId { get; set; }
    public MovieListDTO MovieList { get; set; }
    public int MovieId { get; set; }
    public MovieDTO Movie { get; set; }
}