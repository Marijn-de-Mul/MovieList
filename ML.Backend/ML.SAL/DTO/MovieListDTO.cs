using ML.SAL.Interfaces;

namespace ML.SAL.DTO;

public class MovieListDTO : IMovieList
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int userId { get; set; }
    public List<MovieDTO> Movies { get; set; }
    public List<UserDTO> SharedWith { get; set; }
}