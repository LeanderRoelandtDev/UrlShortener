using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models
{
    public class User
    {
        public Guid ID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}