using System.IdentityModel.Tokens.Jwt;
using UrlShortener.Dtos.Auth.Response;
using System.Net;
using System.Net.Http.Json;

namespace UrlShortener.Testing.Integration.AuthIntegrationTests
{
    public class LoginTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
    {
        [Fact]
        public async Task Login_ValidCredentials_ReturnsOk()
        {
            HttpClient client = factory.CreateClient();

            DateTime now = DateTime.UtcNow;

            HttpResponseMessage responseLogin = await client.PostAsJsonAsync("/api/Auth/login", new
            {
                Username = "Testing",
                Password = "Test123!",
            });

            Assert.Equal(HttpStatusCode.OK, responseLogin.StatusCode);

            LoginResponse result = await responseLogin.Content.ReadFromJsonAsync<LoginResponse>();

            Assert.NotNull(result);

            Assert.False(string.IsNullOrWhiteSpace(result.JwtToken));

            JwtSecurityTokenHandler handler = new();

            Assert.True(handler.CanReadToken(result.JwtToken));

            JwtSecurityToken token = handler.ReadJwtToken(result.JwtToken);

            Assert.NotNull(token);
            Assert.True(token.ValidTo > DateTime.UtcNow);
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task Login_UserDoesNotExist_ReturnsBadRequest(string? username)
        {
            HttpClient client = factory.CreateClient();


            HttpResponseMessage responseLogin = await client.PostAsJsonAsync("/api/Auth/login", new
            {
                Username = username,
                Password = "Test123!",
            });

            Assert.Equal(HttpStatusCode.BadRequest, responseLogin.StatusCode);
        }


        [Fact]
        public async Task Login_UserDoesNotExist_ReturnsNotFound()
        {
            HttpClient client = factory.CreateClient();


            HttpResponseMessage responseLogin = await client.PostAsJsonAsync("/api/Auth/login", new
            {
                Username = "ThisUsernameDoesntExist",
                Password = "Test123!",
            });

            Assert.Equal(HttpStatusCode.NotFound, responseLogin.StatusCode);
        }


        [Fact]
        public async Task Login_InvalidPassword_ReturnsUnauthorized()
        {
            HttpClient client = factory.CreateClient();

            HttpResponseMessage responseLogin = await client.PostAsJsonAsync("/api/Auth/login", new
            {
                Username = "Testing",
                Password = "WrongPassword",
            });

            Assert.Equal(HttpStatusCode.Unauthorized, responseLogin.StatusCode);
        }
    }
}