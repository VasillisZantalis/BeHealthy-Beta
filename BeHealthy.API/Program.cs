using System.Text.Json.Serialization;
using BeHealthy.API.Middleware;
using BeHealthy.Application;
using BeHealthy.Domain.Entities;
using BeHealthy.Infrastructure;
using BeHealthy.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddOpenApi();

// RFC 7807 problem details for every non-2xx response (validation failures, explicit
// Problem()/NotFound()/etc. calls, and unhandled exceptions via GlobalExceptionHandler).
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddCors(options => {
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