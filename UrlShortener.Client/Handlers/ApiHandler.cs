using System.Net.Http.Json;
using UrlShortener.Dtos.Auth.Response;

namespace UrlShortener.Client.Handlers
{
    public class ApiHandler(IHttpClientFactory httpClientFactory)
    {
        private const string BaseUrl = "/api/";

        public async Task<T?> Send<T>(HttpMethod method, string path, object? request = null) where T : new()
        {
            HttpClient httpClient = httpClientFactory.CreateClient("UrlShortenerApi");

            HttpRequestMessage message = new HttpRequestMessage(method, BaseUrl + path);

            if (request is not null)
            {
                message.Content = JsonContent.Create(request);
            }

            HttpResponseMessage response = await httpClient.SendAsync(message);

            response.EnsureSuccessStatusCode();

            T? result = await response.Content.ReadFromJsonAsync<T>();

            return result ?? new T();
        }
    }
}