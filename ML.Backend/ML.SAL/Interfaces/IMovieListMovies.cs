using ML.SAL.DTO;

namespace ML.SAL.Interfaces;

public interface IMovieListMovies
{
    int MovieListId { get; set; }
    MovieListDTO MovieList { get; set; }
    int MovieId { get; set; }
    MovieDTO Movie { get; set; }
}