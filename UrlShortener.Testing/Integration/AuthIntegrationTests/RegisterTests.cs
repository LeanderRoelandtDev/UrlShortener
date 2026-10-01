using System.Net;
using System.Net.Http.Json;
using UrlShortener.Dtos.Auth.Request;

namespace UrlShortener.Testing.Integration.AuthIntegrationTests
{
    public class RegisterTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
    {
        [Fact]
        public async Task Register_ValidUser_ReturnsOk()
        {
            HttpClient client = factory.CreateClient();

            DateTime now = DateTime.UtcNow;

            HttpResponseMessage responseRegister = await client.PostAsJsonAsync("/api/Auth/register", new
            {
                Username = $"Testing{now:yyyyMMdd_HHmmss}_{now:fff}",
                Email = $"testing{now:yyyyMMdd_HHmmss}_{now:fff}@test.com",
                Password = "Test123!",
                ConfirmPassword = "Test123!"
            });

            Assert.Equal(HttpStatusCode.OK, responseRegister.StatusCode);
        }


        [Fact]
        public async Task Register_DuplicateUsername_ReturnsConflict()
        {
            HttpClient client = factory.CreateClient();

            DateTime now = DateTime.UtcNow;

            HttpResponseMessage responseRegister = await client.PostAsJsonAsync("/api/Auth/register", new
            {
                Username = "Testing",
                Email = $"testing{now:yyyyMMdd_HHmmss}_{now:fff}@test.com",
                Password = "Test123!",
                ConfirmPassword = "Test123!"
            });

            Assert.Equal(HttpStatusCode.Conflict, responseRegister.StatusCode);
        }


        [Fact]
        public async Task Register_DuplicateEmail_ReturnsConflict()
        {
            HttpClient client = factory.CreateClient();

            DateTime now = DateTime.UtcNow;

            HttpResponseMessage responseRegister = await client.PostAsJsonAsync("/api/Auth/register", new
            {
                Username = $"Testing{now:yyyyMMdd_HHmmss}_{now:fff}",
                Email = "Testing@Testing.com",
                Password = "Test123!",
                ConfirmPassword = "Test123!"
            });

            Assert.Equal(HttpStatusCode.Conflict, responseRegister.StatusCode);
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task Register_InvalidUsername_ReturnsBadRequest(string? username)
        {
            HttpClient client = factory.CreateClient();

            DateTime now = DateTime.UtcNow;

            HttpResponseMessage responseRegister = await client.PostAsJsonAsync("/api/Auth/register", new
            {
                Username = username,
                Email = $"testing{now:yyyyMMdd_HHmmss}_{now:fff}@test.com",
                Password = "Test123!",
                ConfirmPassword = "Test123!"
            });

            Assert.Equal(HttpStatusCode.BadRequest, responseRegister.StatusCode);
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("not-an-email")]
        [InlineData("test@")]
        [InlineData("@test.com")]
        [InlineData("test.com")]
        public async Task Register_InvalidEmail_ReturnsBadRequest(string? email)
        {
            HttpClient client = factory.CreateClient();

            DateTime now = DateTime.UtcNow;

            HttpResponseMessage responseRegister = await client.PostAsJsonAsync("/api/Auth/register", new
            {
                Username = $"Testing{now:yyyyMMdd_HHmmss}_{now:fff}",
                Email = email,
                Password = "Test123!",
                ConfirmPassword = "Test123!"
            });

            Assert.Equal(HttpStatusCode.BadRequest, responseRegister.StatusCode);
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("Test1!")]
        [InlineData("test123!")]
        [InlineData("TEST123!")]
        [InlineData("TestTest!")]
        [InlineData("Test1234")]
        public async Task Register_InvalidPassword_ReturnsBadRequest(string? password)
        {
            HttpClient client = factory.CreateClient();

            DateTime now = DateTime.UtcNow;

            HttpResponseMessage responseRegister = await client.PostAsJsonAsync("/api/Auth/register", new
            {
                Username = $"Testing{now:yyyyMMdd_HHmmss}_{now:fff}",
                Email = $"testing{now:yyyyMMdd_HHmmss}_{now:fff}@test.com",
                Password = password,
                ConfirmPassword = "Test123!"
            });

            Assert.Equal(HttpStatusCode.BadRequest, responseRegister.StatusCode);
        }


        [Theory]
        [InlineData("Test123!", "Test1234!")]
        [InlineData("Test123!", "Test123@")]
        [InlineData("Test123!", "test123!")]
        public async Task Register_PasswordsDoNotMatch_ReturnsBadRequest(string password,string confirmPassword)
        {
            HttpClient client = factory.CreateClient();

            DateTime now = DateTime.UtcNow;

            HttpResponseMessage responseRegister = await client.PostAsJsonAsync("/api/Auth/register", new
            {
                Username = $"Testing{now:yyyyMMdd_HHmmss}_{now:fff}",
                Email = $"testing{now:yyyyMMdd_HHmmss}_{now:fff}@test.com",
                Password = password,
                ConfirmPassword = confirmPassword
            });

            Assert.Equal(HttpStatusCode.BadRequest, responseRegister.StatusCode);
        }
    }
}