using ML.SAL.DTO;
using ML.SAL.Interfaces;

namespace ML.SAL.Models
{
    public class MovieList : IMovieList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int userId { get; set; }
        public ICollection<MovieListMoviesDTO> Movies { get; set; }
        public ICollection<MovieListSharedWithDTO> SharedWith { get; set; }
    }
}