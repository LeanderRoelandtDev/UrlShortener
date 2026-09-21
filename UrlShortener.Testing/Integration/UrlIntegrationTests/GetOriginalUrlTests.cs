using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace UrlShortener.Testing.Integration.UrlIntegrationTests
{
    public class GetOriginalUrlTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
    {
        [Fact]
        public async Task GetOriginalUrl_ReturnNotNull()
        {
            HttpClient client = factory.CreateClient();

            var response = await client.PostAsJsonAsync("/api/url/GetOriginalUrl", new
            {
                ShortUrl = "d1JVCQz3"
            });

            response.EnsureSuccessStatusCode();

            string result = await response.Content.ReadAsStringAsync();

            Assert.NotNull(result);
        }

        [Theory]
        [InlineData("")]
        [InlineData("abc")]
        [InlineData("abcdefg")]
        [InlineData("abcdefghi")]
        [InlineData("abcdefgh!")]
        public async Task GetOriginalUrl_WithInvalidShortUrl_ReturnsBadRequest(string shortUrl)
        {
            HttpClient client = factory.CreateClient();

            HttpResponseMessage response = await client.PostAsJsonAsync(
                "/api/url/GetOriginalUrl",
                new
                {
                    ShortUrl = shortUrl
                });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}