using System;
using System.Collections.Generic;
using System.Text;

namespace UrlShortener.Models
{
    public class UrlEntity
    {
        public Guid Id { get; set; }
        public required string ShortUrl { get; set; }
        public required string OriginalUrl { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required DateTime UpdatedAt { get; set; }
        public int ClickCount { get; set; } = 0;
        public Guid UserId { get; set; }
    }
}