namespace UrlShortener.Api.Installers
{
    public static class InstallApi
    {
        public static WebApplicationBuilder InstallRestApi(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "UrlShortenerCorsPolicy",
                    policy =>
                    {
                        policy.WithOrigins("https://localhost:7124")
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            return builder;
        }
    }
}