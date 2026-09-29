using Npgsql;
using UrlShortener.Infrastructure.Context;

namespace UrlShortener.Api.Installers
{
    public static class TestingSetupInstaller
    {
        public static WebApplicationBuilder InstallTestingSetup(this WebApplicationBuilder builder)
        {
            builder.Configuration.AddUserSecrets<Program>();

            string connectionString = builder.Configuration.GetConnectionString("UrlShortenerDbContext") ?? throw new InvalidOperationException("Connection string not found.");

            NpgsqlConnectionStringBuilder csb = new(connectionString);
            csb.Database += "_Testing";

            builder.Configuration["ConnectionStrings:" + "UrlShortenerDbContext"] = csb.ConnectionString;

            return builder;
        }
    }
}