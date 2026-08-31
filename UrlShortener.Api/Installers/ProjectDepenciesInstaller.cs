using UrlShortener.Core;
using UrlShortener.Infrastructure;

namespace UrlShortener.Api.Installers
{
    public static class ProjectDepenciesInstaller
    {
        public static WebApplicationBuilder InstallProjectDepencies(this WebApplicationBuilder builder)
        {
            var services = builder.Services;

            //Depency injection of services per project
            services.AddCore();
            services.AddInfrastructure();

            return builder;
        }
    }
}