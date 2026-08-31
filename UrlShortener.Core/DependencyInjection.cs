using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Core.Interfaces.Services;
using UrlShortener.Core.Services;

namespace UrlShortener.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<IUrlService, UrlService>();

            return services;
        }
    }
}