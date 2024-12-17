using ML.SAL.Interfaces;

namespace ML.DAL.Interfaces
{
    public interface IMovieRepository
    {
        void Add(IMovie movie);
        IMovie GetById(int id);
        List<IMovie> Search(string query);
        void Update(IMovie movie);
        void Delete(int id);
    }
}