using ML.SAL.DTO;
using ML.SAL.Interfaces;

namespace ML.SAL.Models;

public class MovieList : IMovieList
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<MovieDTO> Movies { get; set; }
    public List<UserDTO> SharedWith { get; set; }
}