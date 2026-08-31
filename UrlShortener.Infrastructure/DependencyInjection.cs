using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Core.Interfaces.Repositories;
using UrlShortener.Infrastructure.Repositories;

namespace UrlShortener.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IUrlRepository, UrlRepository>();

            return services;
        }
    }
}
