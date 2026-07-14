using BeHealthy.Application;
using BeHealthy.Application.Services;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Components;
using BeHealthy.Components.Account;
using BeHealthy.Domain.Entities;
using BeHealthy.Endpoints.Culture;
using BeHealthy.Infrastructure;
using BeHealthy.Infrastructure.Data;
using BeHealthy.Services;
using BeHealthy.Services.Interfaces;
using BeHealthy.States;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddScoped<IModalService, ModalService>();

builder.Services.AddScoped<ModalStateService>();
builder.Services.AddScoped<NavMenuState>();
builder.Services.AddScoped<LoaderServiceState>();
builder.Services.AddScoped<BreadcrumbServiceState>();
builder.Services.AddScoped<AlertModalStateService>();
builder.Services.AddSingleton<ToastService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
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

app.Run();