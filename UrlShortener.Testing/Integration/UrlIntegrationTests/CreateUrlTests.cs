using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using UrlShortener.Dtos.Auth.Response;

namespace UrlShortener.Testing.Integration.UrlIntegrationTests
{
    public class CreateUrlTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
    {
        [Fact]
        public async Task CreateUrl_ThenGetOriginalUrl_ReturnsOriginalUrl()
        {
            HttpClient client = factory.CreateClient();

            await LoginUsingTestAccount(client);

            HttpResponseMessage responseCreateUrl = await client.PostAsJsonAsync("/api/url/CreateShortUrl", new
            {
                Url = "https://google.com"
            });

            responseCreateUrl.EnsureSuccessStatusCode();

            string resultCreateUrl = await responseCreateUrl.Content.ReadAsStringAsync();

            HttpResponseMessage responseGetUrl = await client.PostAsJsonAsync("/api/url/GetOriginalUrl", new
            {
                ShortUrl = resultCreateUrl
            });

            responseGetUrl.EnsureSuccessStatusCode();

            string resultGetUrl = await responseGetUrl.Content.ReadAsStringAsync();

            Assert.Equal("https://google.com", resultGetUrl);
        }


        [Fact]
        public async Task CreateUrl_ReturnsShortUrl()
        {
            HttpClient client = factory.CreateClient();

            await LoginUsingTestAccount(client);

            HttpResponseMessage response = await client.PostAsJsonAsync("/api/url/CreateShortUrl", new
            {
                Url = "https://google.com"
            });

            response.EnsureSuccessStatusCode();

            string result = await response.Content.ReadAsStringAsync();

            Assert.NotNull(result);
            Assert.Equal(8, result.Length);
        }

        [Theory]
        [InlineData("not-a-url")]
        [InlineData("google.com")]
        [InlineData("")]
        public async Task CreateUrl_WithInvalidUrl_ReturnsBadRequest(string url)
        {
            HttpClient client = factory.CreateClient();

            await LoginUsingTestAccount(client);

            HttpResponseMessage response = await client.PostAsJsonAsync("/api/url/CreateShortUrl", new
            {
                Url = url
            });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }


        private async Task<HttpClient> LoginUsingTestAccount(HttpClient client)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("/api/auth/login", new
            {
                Username = "Testing",
                Password = "Test123!"
            });

            LoginResponse loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>() ?? throw new InvalidOperationException("LoginResponse was empty.");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponse.JwtToken);

            return client;
        }
    }
}