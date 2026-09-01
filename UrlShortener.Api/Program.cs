using UrlShortener.Api.Installers;
using UrlShortener.Infrastructure.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.InstallSwagger()
       .InstallProjectDependencies();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();

    var db = scope.ServiceProvider
        .GetRequiredService<UrlShortenerDbContext>();

    Console.WriteLine(db.Database.CanConnect());
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();