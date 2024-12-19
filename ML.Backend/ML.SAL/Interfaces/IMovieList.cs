using ML.SAL.DTO;

namespace ML.SAL.Interfaces;

public interface IMovieList
{
    int Id { get; set; }
    string Name { get; set; }
    int userId { get; set; }
    ICollection<MovieListMoviesDTO> Movies { get; set; }
    ICollection<MovieListSharedWithDTO> SharedWith { get; set; }
}