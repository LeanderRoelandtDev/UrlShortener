using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using UrlShortener.Infrastructure.Context;

namespace UrlShortener.Testing.Integration
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public CustomWebApplicationFactory()
        {
            IConfigurationRoot secrets = new ConfigurationBuilder()
                    .AddUserSecrets<Program>()
                    .Build();

            string original = secrets.GetConnectionString(nameof(UrlShortenerDbContext)) ?? throw new InvalidOperationException("Connection string not found in user secrets.");

            NpgsqlConnectionStringBuilder csb = new NpgsqlConnectionStringBuilder(original);
            csb.Database += "_Testing";

            Environment.SetEnvironmentVariable($"ConnectionStrings__{nameof(UrlShortenerDbContext)}", csb.ConnectionString);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
        }
    }
}