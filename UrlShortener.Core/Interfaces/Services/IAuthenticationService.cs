using UrlShortener.Models;

namespace UrlShortener.Core.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task<string> GenerateJWTToken(User user);
    }
}