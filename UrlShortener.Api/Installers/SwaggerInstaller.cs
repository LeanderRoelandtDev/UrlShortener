using Microsoft.OpenApi;

namespace UrlShortener.Api.Installers
{
    public static class SwaggerInstaller
    {
        public static WebApplicationBuilder InstallSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddOpenApi();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "UrlShortener API",
                    Version = "v1"
                });
            });

            return builder;
        }
    }
}