using UrlShortener.Core.Exceptions;
using UrlShortener.Core.Interfaces.Repositories;
using UrlShortener.Core.Interfaces.Services;
using UrlShortener.Dtos.Auth.Request;
using UrlShortener.Dtos.Auth.Response;
using UrlShortener.Models;

namespace UrlShortener.Core.Services
{
    internal class AuthService(IAuthRepository authRepository, IAuthenticationService authenticationService) : IAuthService
    {
        public async Task Register(RegisterRequest request)
        {
            request.Username = request.Username.Trim();

            bool alreadyExists = await authRepository.UserExistsByEmailOrUsername(request.Email, request.Username);

            if (alreadyExists)
            {
                throw ErrorException.AlreadyExists("Email or username is already in use");
            }

            User user = new User
            {
                UserName = request.Username,
                Email = request.Email
            };

            await authRepository.CreateUser(user, request.Password);
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            User user = await authRepository.GetUserByUserName(request.UserName);

            if (user == null) throw ErrorException.NotFound("User not found");


            bool isValidPassword = await authRepository.CheckPassword(user.ID, request.Password);

            if (!isValidPassword) throw ErrorException.Unauthorized("Invalid Credentials");

            return new LoginResponse
            {
                JwtToken = await authenticationService.GenerateJWTToken(user),
            };
        }
    }
}