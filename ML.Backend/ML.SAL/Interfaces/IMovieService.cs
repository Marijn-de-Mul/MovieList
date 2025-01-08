using ML.SAL.DTO;

namespace ML.SAL.Interfaces;

public interface IMovieService
{
    void AddMovie(MovieDTO movie);
    MovieDTO GetMovieById(int id);
    Task<List<MovieDTO>> SearchMovies(string query, string region, string language);
    void UpdateMovie(MovieDTO movie);
    void DeleteMovie(int id);
}