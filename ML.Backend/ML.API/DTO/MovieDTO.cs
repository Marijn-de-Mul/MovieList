namespace ML.API.DTO;

public class MovieDTO
{
    public int Id { get; set; }
    public int TheMovieDbId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string? BannerUrl { get; set; }
}