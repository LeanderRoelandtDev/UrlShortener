using UrlShortener.Dtos.Auth.Request;
using UrlShortener.Dtos.Auth.Response;

namespace UrlShortener.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(LoginRequest request);
        Task Register(RegisterRequest request);
    }
}