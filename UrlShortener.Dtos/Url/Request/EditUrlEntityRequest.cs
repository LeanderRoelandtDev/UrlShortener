using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Dtos.Url.Request
{
    public class EditUrlEntityRequest
    {
        [Required]
        [RegularExpression("^[A-Za-z0-9]{8}$")]
        public string ShortUrl { get; set; }

        [Required]
        [Url]
        public string NewUrl { get; set; }
        public int ClickCount { get; set; }
    }
}