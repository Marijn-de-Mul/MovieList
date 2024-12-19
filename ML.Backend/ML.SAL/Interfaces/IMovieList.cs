using ML.SAL.DTO;

namespace ML.SAL.Interfaces;

public interface IMovieList
{
    int Id { get; set; }
    string Name { get; set; }
    int userId { get; set; }
    List<MovieDTO> Movies { get; set; }
    List<UserDTO> SharedWith { get; set; }
}