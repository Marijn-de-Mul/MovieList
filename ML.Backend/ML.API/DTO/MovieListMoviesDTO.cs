namespace ML.API.DTO;

public class MovieListMoviesDTO
{
    public int MovieListId { get; set; }
    public MovieListDTO MovieList { get; set; }
    public int MovieId { get; set; }
    public MovieDTO Movie { get; set; }
}