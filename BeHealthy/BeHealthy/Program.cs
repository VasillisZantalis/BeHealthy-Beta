using BeHealthy.Application;
using BeHealthy.Components;
using BeHealthy.Components.Account;
using BeHealthy.Endpoints.Culture;
using BeHealthy.Infrastructure;
using BeHealthy.Infrastructure.Data;
using BeHealthy.States;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
}).AddIdentityCookies();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/accessdenied";
    options.Cookie.Name = "auth_cookie";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.LoginPath = "/login";
});

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("log/log.txt", rollingInterval: RollingInterval.Day)
    .CreateBootstrapLogger();

builder.Host.UseSerilog();

builder.Services.AddLocalization();

string[] supportedCultures = ["en-US", "el-GR"];
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

var provider = localizationOptions.RequestCultureProviders
    .FirstOrDefault(p => p is AcceptLanguageHeaderRequestCultureProvider);

if (provider != null)
{
    localizationOptions.RequestCultureProviders.Remove(provider);
}

builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// States
builder.Services.AddScoped<ModalStateService>();
builder.Services.AddScoped<NavMenuState>();
builder.Services.AddScoped<LoaderServiceState>();
builder.Services.AddScoped<BreadcrumbServiceState>();
builder.Services.AddScoped<ConfirmDeleteStateService>();
builder.Services.AddScoped<ToastrStateService>();
builder.Services.AddScoped<AlertModalStateService>();

var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ApplicationDbContext>();
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }
    }
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseRequestLocalization(localizationOptions);

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapAdditionalIdentityEndpoints();
app.MapCultureEndpoints();
//app.MapAppointmentsEndpoints();
//app.MapDepartmentEndpoints();
//app.MapDoctorsEndpoints();
//app.MapNursesEndpoints();
//app.MapPatientEndpoints();
//app.MapPrescriptionsEndpoints();
//app.MapRoomsEndpoints();
//app.MapUsersEndpoints();

app.Run();