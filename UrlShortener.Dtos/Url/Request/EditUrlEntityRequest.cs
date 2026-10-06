namespace UrlShortener.Dtos.Url.Request
{
    public class EditUrlEntityRequest
    {
        public string ShortUrl { get; set; }
        public string NewUrl { get; set; }
        public int ClickCount { get; set; }
    }
}