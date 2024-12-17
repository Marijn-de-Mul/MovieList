namespace ML.SAL.Interfaces;

public interface IMovie
{
    int Id { get; set; }
    int TheMovieDbId { get; set; }
    string Title { get; set; }
    string Description { get; set; }
}