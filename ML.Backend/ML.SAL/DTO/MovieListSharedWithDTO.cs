namespace ML.SAL.DTO;

public class MovieListSharedWithDTO
{
    public int MovieListId { get; set; }
    public MovieListDTO? MovieList { get; set; }
    public int UserId { get; set; }
    public UserDTO User { get; set; }
}