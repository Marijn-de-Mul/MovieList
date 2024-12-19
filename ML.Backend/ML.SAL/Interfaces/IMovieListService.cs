using ML.SAL.DTO;

namespace ML.SAL.Interfaces;

public interface IMovieListService
{
    List<MovieListDTO> GetListsByUser(int userId);
    void CreateList(MovieListDTO list);
    void EditList(int id, MovieListDTO list, int userId);
    void DeleteList(int id);
    void ShareList(int id, UserDTO user);
    void RemoveUserFromList(int id, int userId);
}