using System.Net.Http.Json;
using UrlShortener.Dtos.Auth.Response;

namespace UrlShortener.Client.Handlers
{
    public class ApiHandler(IHttpClientFactory httpClientFactory)
    {
        private const string BaseUrl = "/api/";

        public async Task<LoginResponse?> Send<T>(HttpMethod method, string path, object? request = null)
        {
            HttpClient httpClient = httpClientFactory.CreateClient("UrlShortenerApi");

            var response = await httpClient.PostAsJsonAsync(BaseUrl + path, request);

            Console.WriteLine(BaseUrl + path);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

            return result;
        }
    }
}