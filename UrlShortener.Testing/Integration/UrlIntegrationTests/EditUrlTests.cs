using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using UrlShortener.Dtos.Auth.Response;

namespace UrlShortener.Testing.Integration.UrlIntegrationTests
{
    public class EditUrlTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
    {
        [Fact]
        public async Task EditUrl_ReturnsNoContent()
        {
            HttpClient client = factory.CreateClient();

            await LoginUsingTestAccount(client);

            HttpResponseMessage responseCreateUrl = await client.PostAsJsonAsync("/api/url/CreateShortUrl", new
            {
                Url = "https://google.com"
            });

            responseCreateUrl.EnsureSuccessStatusCode();

            string shortUrl = await responseCreateUrl.Content.ReadAsStringAsync();

            HttpResponseMessage responseEditUrl = await client.PutAsJsonAsync("/api/url/EditShortUrl",new
            {
                ShortUrl = shortUrl,
                NewUrl = "https://youtube.com"
            });

            Assert.Equal(HttpStatusCode.NoContent, responseEditUrl.StatusCode);
        }

        [Fact]
        public async Task EditUrl_ThenGetOriginalUrl_ReturnsNewUrl()
        {
            HttpClient client = factory.CreateClient();

            await LoginUsingTestAccount(client);

            HttpResponseMessage responseCreateUrl = await client.PostAsJsonAsync("/api/url/CreateShortUrl", new
            {
                Url = "https://google.com"
            });

            responseCreateUrl.EnsureSuccessStatusCode();

            string shortUrl = await responseCreateUrl.Content.ReadAsStringAsync();

            HttpResponseMessage responseEditUrl = await client.PutAsJsonAsync("/api/url/EditShortUrl",new
            {
                ShortUrl = shortUrl,
                NewUrl = "https://youtube.com"
            });

            responseEditUrl.EnsureSuccessStatusCode();

            HttpResponseMessage responseGetUrl = await client.PostAsJsonAsync("/api/url/GetOriginalUrl", new
            {
                ShortUrl = shortUrl
            });

            responseGetUrl.EnsureSuccessStatusCode();

            string result = await responseGetUrl.Content.ReadAsStringAsync();

            Assert.Equal("https://youtube.com", result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("abc")]
        [InlineData("abcdefg")]
        [InlineData("abcdefg!")]
        [InlineData("abcdefghi")]
        public async Task EditUrl_WithInvalidShortUrl_ReturnsBadRequest(string shortUrl)
        {
            HttpClient client = factory.CreateClient();

            await LoginUsingTestAccount(client);

            HttpResponseMessage response = await client.PutAsJsonAsync("/api/url/EditShortUrl", new
            {
                ShortUrl = shortUrl,
                NewUrl = "https://youtube.com"
            });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("not-a-url")]
        [InlineData("google.com")]
        public async Task EditUrl_WithInvalidNewUrl_ReturnsBadRequest(string newUrl)
        {
            HttpClient client = factory.CreateClient();

            await LoginUsingTestAccount(client);

            HttpResponseMessage response = await client.PutAsJsonAsync("/api/url/EditShortUrl", new
            {
                ShortUrl = "ZZzz1234",
                NewUrl = newUrl
            });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task EditUrl_WhenShortUrlDoesNotExist_ReturnsNotFound()
        {
            HttpClient client = factory.CreateClient();

            await LoginUsingTestAccount(client);

            HttpResponseMessage response = await client.PutAsJsonAsync("/api/url/EditShortUrl", new
            {
                //Make sure this shortUrl never exists in the database
                ShortUrl = "ZZzz1234",
                NewUrl = "https://youtube.com"
            });

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task EditUrl_WhenNotAuthenticated_ReturnsUnauthorized()
        {
            HttpClient client = factory.CreateClient();

            HttpResponseMessage response = await client.PutAsJsonAsync("/api/url/EditShortUrl", new
            {
                ShortUrl = "ZZzz1234",
                NewUrl = "https://youtube.com"
            });

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
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