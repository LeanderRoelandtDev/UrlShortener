using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Dtos.Url.Request
{
    public class GetOriginalUrlRequest
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]{8}$")]
        public string ShortUrl { get; set; }
    }
}