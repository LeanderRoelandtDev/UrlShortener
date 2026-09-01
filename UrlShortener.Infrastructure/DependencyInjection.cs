using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Core.Interfaces.Repositories;
using UrlShortener.Infrastructure.Context;
using UrlShortener.Infrastructure.Repositories;

namespace UrlShortener.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //Database Context
            var connectionString = configuration.GetConnectionString(nameof(UrlShortenerDbContext));

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException($"Connection string '{nameof(UrlShortenerDbContext)}' not found.");

            services.AddDbContext<UrlShortenerDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });


            //Services
            services.AddScoped<IUrlRepository, UrlRepository>();

            return services;
        }
    }
}
