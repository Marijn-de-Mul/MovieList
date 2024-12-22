using ML.SAL.DTO;
using ML.SAL.Interfaces;

namespace ML.DAL.Interfaces;

public interface IMovieListRepository
{
    List<IMovieList> GetListsByUser(int userId);
    void CreateList(IMovieList list);
    void EditList(int id, IMovieList list);
    void DeleteList(int id);
    void ShareList(int id, IUser user);
    void RemoveUserFromList(int id, int userId);
    
    void RemoveMovieFromList(int listId, int movieId);
}