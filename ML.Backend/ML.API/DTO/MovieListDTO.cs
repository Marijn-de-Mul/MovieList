namespace ML.API.DTO;

public class MovieListDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<MovieDTO> Movies { get; set; }
    public List<UserDTO> SharedWith { get; set; }
}