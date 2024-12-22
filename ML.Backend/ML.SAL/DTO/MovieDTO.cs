using ML.SAL.Interfaces;

namespace ML.SAL.DTO;

public class MovieDTO : IMovie
{
    public int Id { get; set; }
    public int TheMovieDbId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string? BannerUrl { get; set; } = "https://via.placeholder.com/200x300";
}