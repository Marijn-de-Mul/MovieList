using ML.SAL.DTO;

namespace ML.SAL.Interfaces;

public interface IMovieListService
{
    void CreateList(MovieListDTO list);
    void EditList(int id, MovieListDTO list);
    void DeleteList(int id);
    void ShareList(int id, UserDTO user);
    void RemoveUserFromList(int id, int userId);
}