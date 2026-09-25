using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Dtos.Auth.Request
{
    public class LoginRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}