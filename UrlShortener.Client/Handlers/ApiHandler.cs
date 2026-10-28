using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Http;


namespace UrlShortener.Client.Handlers
{
    public class ApiHandler(IHttpClientFactory httpClientFactory)
    {
        private const string BaseUrl = "/api/";
        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
        private record ProblemResponse(string? Detail);

        public async Task<ApiResult<T>> Send<T>(HttpMethod method, string path, object? request = null) where T : new()
        {
            try
            {
                HttpClient httpClient = httpClientFactory.CreateClient("UrlShortenerApi");

                HttpRequestMessage message = new(method, BaseUrl + path);
                message.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);


                if (request is not null)
                {
                    message.Content = JsonContent.Create(request);
                }

                HttpResponseMessage response = await httpClient.SendAsync(message);
                string body = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    string? detail = string.IsNullOrWhiteSpace(body) ? null : JsonSerializer.Deserialize<ProblemResponse>(body, JsonOptions)?.Detail;

                    return new(default, detail ?? "Something went wrong.");
                }

                T? result = string.IsNullOrWhiteSpace(body) ? new T() : JsonSerializer.Deserialize<T>(body, JsonOptions);

                return new(result ?? new T(), null);
            }
            catch (HttpRequestException)
            {
                return new(default, "Can't reach the server. Try again.");
            }
            catch (JsonException)
            {
                return new(default, "Something went wrong. Try again.");
            }
        }

        public Task<ApiResult<object>> Send(HttpMethod method, string path, object? request = null)=> Send<object>(method, path, request);
    }

    public record ApiResult<T>(T? Data, string? Error)
    {
        public bool Success => Error is null;
    }
}