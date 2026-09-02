namespace UrlShortener.Infrastructure.DomainModels
{
    internal class UrlEntity
    {
        public Guid Id { get; set; }
        public string ShortUrl { get; set; }
        public string OriginalUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public int ClickCount { get; set; } = 0;
    }
}