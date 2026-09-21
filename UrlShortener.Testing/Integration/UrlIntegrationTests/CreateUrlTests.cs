using System.Net;
using System.Net.Http.Json;

namespace UrlShortener.Testing.Integration.UrlIntegrationTests
{
    public class CreateUrlTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
    {
        [Fact]
        public async Task CreateUrl_ThenGetOriginalUrl_ReturnsOriginalUrl()
        {
            HttpClient client = factory.CreateClient();

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

            HttpResponseMessage response = await client.PostAsJsonAsync("/api/url/CreateShortUrl", new
            {
                Url = url
            });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}