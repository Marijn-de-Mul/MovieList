using ML.SAL.Interfaces;

namespace ML.SAL.DTO;

public class MovieListDTO : IMovieList
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int userId { get; set; }
    public ICollection<MovieListMoviesDTO> Movies { get; set; }
    public ICollection<MovieListSharedWithDTO> SharedWith { get; set; }
}