namespace ML.API.DTO;

public class MovieListDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int userId { get; set; }
    public List<MovieListMoviesDTO> Movies { get; set; }
    public List<MovieListSharedWithDTO> SharedWith { get; set; }
}