using UrlShortener.Api.Exceptions;
using UrlShortener.Api.Installers;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsEnvironment("Testing"))
{
    builder.InstallTestingSetup();
}

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

public partial class Program { }