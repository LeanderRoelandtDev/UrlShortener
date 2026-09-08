using UrlShortener.Api.Exceptions;
using UrlShortener.Api.Installers;
using UrlShortener.Infrastructure.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.InstallSwagger()
       .InstallProjectDependencies();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ExceptionHandler>();


var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();