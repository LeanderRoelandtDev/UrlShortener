using UrlShortener.Core.Interfaces.Repositories;
using UrlShortener.Core.Interfaces.Services;
using UrlShortener.Dtos.Auth.Request;
using UrlShortener.Dtos.Auth.Response;

namespace UrlShortener.Core.Services
{
    internal class AuthService(IAuthRepository authRepository) : IAuthService
    {
        public async Task<LoginResponse> Login(LoginRequest request)
        {
            return new LoginResponse
            {
                JwtToken = "Test"
            };
        }

        public async Task Register(RegisterRequest request)
        { 
             
        }
    }
}