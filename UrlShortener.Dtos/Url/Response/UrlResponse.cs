namespace UrlShortener.Dtos.Url.Response
{
    public class UrlResponse
    {
        public Guid Id { get; set; }
        public required string ShortUrl { get; set; }
        public required string OriginalUrl { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required DateTime UpdatedAt { get; set; }
        public int ClickCount { get; set; } = 0;
    }
}