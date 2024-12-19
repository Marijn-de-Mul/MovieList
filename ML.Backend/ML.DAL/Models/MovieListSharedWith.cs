using ML.SAL.DTO;
using ML.SAL.Interfaces;

namespace ML.SAL.Models;

public class MovieListSharedWith : IMovieListSharedWith
{
    public int MovieListId { get; set; }
    public MovieListDTO? MovieList { get; set; }
    public int UserId { get; set; }
    public UserDTO User { get; set; }
}