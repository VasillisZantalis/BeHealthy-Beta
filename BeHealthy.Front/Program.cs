using BeHealthy.Front;
using BeHealthy.Front.Common;
using BeHealthy.Front.Components;
using BeHealthy.Front.Services.Auth;
using BeHealthy.Shared.Dtos.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpContextAccessor();

// Cookie holds the signed-in identity for the browser; the JWT it carries as a claim is what
// actually authorizes calls to the API (see ApiClientBase).
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "BeHealthy.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.LoginPath = RoutingEndpoints.LOGIN_PAGE;
        options.AccessDeniedPath = RoutingEndpoints.LOGIN_PAGE;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();

// Named HttpClient for the BeHealthy API
builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("API:BaseUrl")!);
});

builder.Services.AddFrontServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Plain (non-Blazor) endpoints for sign-in/out. The Login page is rendered interactively, so its
// form must post here instead of relying on an EditForm submit, which never gets a live
// HttpContext to write the Set-Cookie response header to once the circuit is running.
app.MapPost("/account/login", async (
        HttpContext httpContext,
        [FromForm] string username,
        [FromForm] string password,
        [FromForm] string? returnUrl,
        IHttpClientFactory httpClientFactory) =>
    {
        var client = httpClientFactory.CreateClient("API");
        var apiResponse = await client.PostAsJsonAsync("auth/login", new LoginRequest { Username = username, Password = password }, ApiJsonOptions.Default);

        var failureRedirect = $"{RoutingEndpoints.LOGIN_PAGE}?error=1" +
            (string.IsNullOrEmpty(returnUrl) ? "" : $"&returnUrl={Uri.EscapeDataString(returnUrl)}");

        if (!apiResponse.IsSuccessStatusCode)
            return Results.Redirect(failureRedirect);

        var loginResult = await apiResponse.Content.ReadFromJsonAsync<LoginResponse>(ApiJsonOptions.Default);
        if (loginResult is null)
            return Results.Redirect(failureRedirect);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, loginResult.User.Id),
            new(ClaimTypes.Name, loginResult.User.Username),
            new(ClaimTypes.GivenName, loginResult.User.FirstName),
            new(ClaimTypes.Surname, loginResult.User.LastName),
            new(ClaimTypes.Role, loginResult.User.Role.ToString()),
            new(AuthClaimTypes.ApiToken, loginResult.Token)
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = loginResult.ExpiresAtUtc
        });

        return Results.Redirect(string.IsNullOrEmpty(returnUrl) ? RoutingEndpoints.HOME_PAGE : returnUrl);
    })
    .AllowAnonymous()
    .DisableAntiforgery();

app.MapPost("/account/logout", async (HttpContext httpContext) =>
    {
        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Results.Redirect(RoutingEndpoints.LOGIN_PAGE);
    })
    .AllowAnonymous()
    .DisableAntiforgery();

app.Run();
