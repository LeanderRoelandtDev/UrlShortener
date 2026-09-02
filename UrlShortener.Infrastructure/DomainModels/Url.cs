
namespace UrlShortener.Infrastructure.DomainModels
{
    internal class Url
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string OriginalUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public int ClickCount { get; set; } = 0;
    }
}