using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using UrlShortener.Client.Handlers;

namespace UrlShortener.Client.Installers
{
    public static class ApiInstaller
    {
        public static WebAssemblyHostBuilder InstallApi(this WebAssemblyHostBuilder builder)
        {
            builder.Services.AddHttpClient("UrlShortenerApi", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7023");
            });

            builder.Services.AddScoped<ApiHandler>();


            return builder;
        }
    }
}