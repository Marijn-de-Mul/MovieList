using ML.SAL.DTO;

namespace ML.SAL.Interfaces;

public interface IMovieListSharedWith
{
    int MovieListId { get; set; }
    MovieListDTO MovieList { get; set; }
    int UserId { get; set; }
    UserDTO User { get; set; }
}