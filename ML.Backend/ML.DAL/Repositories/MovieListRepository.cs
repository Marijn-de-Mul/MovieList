using ML.DAL.Data;
using ML.DAL.Interfaces;

namespace ML.DAL.Repositories;

public class MovieListRepository : IMovieListRepository
{
    private readonly ApplicationDbContext _context;

    public MovieListRepository(ApplicationDbContext context)
    {
        _context = context;
    }
}