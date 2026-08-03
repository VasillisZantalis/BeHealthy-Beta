using BeHealthy.API.Middleware;
using BeHealthy.Application;
using BeHealthy.Infrastructure;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddOpenApi();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddCQRS();

builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorClient", policy =>
        policy.WithOrigins(
                  "https://localhost:7209", // legacy Blazor Server client
                  "https://localhost:7224", // WASM frontend (https)
                  "http://localhost:5005")  // WASM frontend (http)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
            .EnableDarkMode()
            .WithTitle("BeHealthy API")
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });

    await app.Services.InitializeDatabaseAsync();
}

app.UseHttpsRedirection();

app.UseCors("BlazorClient");

app.UseAuthorization();

app.MapControllers();

app.Run();