using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Dtos.Url.Request
{
    public class CreateShortUrlRequest
    {
        [Required]
        [Url]
        public string Url { get; set; } = string.Empty;
    }
}