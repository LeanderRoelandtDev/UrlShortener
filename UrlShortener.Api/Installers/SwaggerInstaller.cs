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


                options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter: Bearer {your token}"
                });

                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("bearer", document)] = []
                });
            });

            return builder;
        }
    }
}