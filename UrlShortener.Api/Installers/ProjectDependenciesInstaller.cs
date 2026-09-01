using UrlShortener.Core;
using UrlShortener.Infrastructure;

namespace UrlShortener.Api.Installers
{
    public static class ProjectDependenciesInstaller
    {
        public static WebApplicationBuilder InstallProjectDependencies(this WebApplicationBuilder builder)
        {
            var services = builder.Services;

            //Depency injection of services per project
            services.AddCore();
            services.AddInfrastructure(builder.Configuration);

            return builder;
        }
    }
}