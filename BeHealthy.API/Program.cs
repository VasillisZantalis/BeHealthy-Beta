using BeHealthy.Application;
using BeHealthy.Infrastructure;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddCors(options => {
    options.AddPolicy("BlazorClient", policy =>
        policy.WithOrigins("https://localhost:7209")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
