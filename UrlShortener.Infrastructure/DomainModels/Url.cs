using System;
using System.Collections.Generic;
using System.Text;

namespace UrlShortener.Infrastructure.DomainModels
{
    internal class Url
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string OriginalUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public int ClickCount { get; set; } = 0;
    }
}