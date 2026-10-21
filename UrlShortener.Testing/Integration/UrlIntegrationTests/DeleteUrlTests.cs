using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using UrlShortener.Dtos.Auth.Response;

namespace UrlShortener.Testing.Integration.UrlIntegrationTests
{
    public class DeleteUrlTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
    {
        [Fact]
        public async Task DeleteUrl_ReturnsNoContent()
        {
            HttpClient client = factory.CreateClient();

            await LoginUsingTestAccount(client, "Testing", "Test123!");

            HttpResponseMessage responseCreateUrl = await client.PostAsJsonAsync("/api/url/CreateShortUrl", new
            {
                Url = "https://google.com"
            });

            responseCreateUrl.EnsureSuccessStatusCode();

            string shortUrl = await responseCreateUrl.Content.ReadAsStringAsync();

            HttpResponseMessage responseDeleteUrl = await client.DeleteAsync($"/api/url/{shortUrl}");

            Assert.Equal(HttpStatusCode.NoContent, responseDeleteUrl.StatusCode);
        }


        [Fact]
        public async Task DeleteUrl_ThenGetOriginalUrl_ReturnsNotFound()
        {
            HttpClient client = factory.CreateClient();

            await LoginUsingTestAccount(client, "Testing", "Test123!");

            HttpResponseMessage responseCreateUrl = await client.PostAsJsonAsync("/api/url/CreateShortUrl", new
            {
                Url = "https://google.com"
            });

            responseCreateUrl.EnsureSuccessStatusCode();

            string shortUrl = await responseCreateUrl.Content.ReadAsStringAsync();



            HttpResponseMessage responseDeleteUrl = await client.DeleteAsync($"/api/url/{shortUrl}");

            responseDeleteUrl.EnsureSuccessStatusCode();


            HttpResponseMessage responseGetUrl = await client.PostAsJsonAsync("/api/url/GetOriginalUrl", new
            {
                ShortUrl = shortUrl
            });

            Assert.Equal(HttpStatusCode.NotFound, responseGetUrl.StatusCode);
        }


        [Theory]
        [InlineData("abc")]
        [InlineData("abcdefg")]
        [InlineData("abcdefg!")]
        [InlineData("abcdefghi")]
        public async Task DeleteUrl_WithInvalidShortUrl_ReturnsBadRequest(string shortUrl)
        {
            HttpClient client = factory.CreateClient();

            await LoginUsingTestAccount(client, "Testing", "Test123!");

            HttpResponseMessage response = await client.DeleteAsync($"/api/url/{shortUrl}");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }


        [Fact]
        public async Task DeleteUrl_WhenShortUrlDoesNotExist_ReturnsNotFound()
        {
            HttpClient client = factory.CreateClient();

            await LoginUsingTestAccount(client, "Testing", "Test123!");

            HttpResponseMessage response = await client.DeleteAsync("/api/url/abcdefgh");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteUrl_WhenNotAuthenticated_ReturnsUnauthorized()
        {
            HttpClient client = factory.CreateClient();

            HttpResponseMessage response = await client.DeleteAsync("/api/url/abcdefgh");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }


        [Fact]
        public async Task DeleteUrl_WhenNotOwner_ReturnsNotFound()
        {
            HttpClient client = factory.CreateClient();

            //Login with account B and try to delete an Url from account A
            await LoginUsingTestAccount(client, "Testing2", "Test123!");

            HttpResponseMessage response = await client.DeleteAsync("/api/url/ZZzz1234");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }


        private async Task<HttpClient> LoginUsingTestAccount(HttpClient client, string username, string password)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("/api/auth/login",
            new
            {
                Username = username,
                Password = password
            });

            LoginResponse loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>() ?? throw new InvalidOperationException("LoginResponse was empty.");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponse.JwtToken);

            return client;
        }
    }
}