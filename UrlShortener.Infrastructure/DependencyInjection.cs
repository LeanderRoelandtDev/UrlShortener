using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Core.Interfaces.Repositories;
using UrlShortener.Core.Interfaces.Services;
using UrlShortener.Infrastructure.Context;
using UrlShortener.Infrastructure.DomainModels;
using UrlShortener.Infrastructure.Repositories;
using UrlShortener.Infrastructure.Services;

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


            //Registers Identity
            services.AddIdentityCore<ApplicationUser>()
            .AddEntityFrameworkStores<UrlShortenerDbContext>();



            //Repositories
            services.AddScoped<IUrlRepository, UrlRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();

            //Services
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            return services;
        }
    }
}