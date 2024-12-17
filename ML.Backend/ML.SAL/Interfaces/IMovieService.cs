using ML.SAL.DTO;

namespace ML.SAL.Interfaces;

public interface IMovieService
{
    void AddMovie(MovieDTO movie);
    MovieDTO GetMovieById(int id);
    Task<List<MovieDTO>> SearchMovies(string query);
    void UpdateMovie(MovieDTO movie);
    void DeleteMovie(int id);
}