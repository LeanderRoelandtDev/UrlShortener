namespace UrlShortener.Infrastructure.DomainModels
{
    internal class UrlEntity
    {
        public Guid Id { get; set; }
        public required string ShortUrl { get; set; }
        public required string OriginalUrl { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required DateTime UpdatedAt { get; set; }
        public int ClickCount { get; set; } = 0;
        public required Guid ApplicationUserId { get; set; }

        public ApplicationUser? ApplicationUser { get; set; }
    }
}