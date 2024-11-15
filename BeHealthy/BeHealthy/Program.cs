using BeHealthy.Application;
using BeHealthy.Components;
using BeHealthy.Components.Account;
using BeHealthy.Endpoints.Appointments;
using BeHealthy.Endpoints.Culture;
using BeHealthy.Endpoints.Department;
using BeHealthy.Endpoints.Doctors;
using BeHealthy.Endpoints.Nurse;
using BeHealthy.Endpoints.Patient;
using BeHealthy.Endpoints.Prescription;
using BeHealthy.Endpoints.Room;
using BeHealthy.Endpoints.User;
using BeHealthy.Infrastructure;
using BeHealthy.States;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.LoginPath = "/login";
});

var connectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddLocalization();

string[] supportedCultures = ["en-US", "el-GR"];
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(connectionString!);

builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// States
builder.Services.AddScoped<ModalStateService>();
builder.Services.AddScoped<PrivilegeStateService>();

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
app.MapAppointmentsEndpoints();
app.MapDepartmentEndpoints();
app.MapDoctorsEndpoints();
app.MapNursesEndpoints();
app.MapPatientEndpoints();
app.MapPrescriptionsEndpoints();
app.MapRoomsEndpoints();
app.MapUsersEndpoints();

app.Run();
