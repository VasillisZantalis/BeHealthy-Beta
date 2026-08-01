using System.Globalization;
using BeHealthy.Frontend;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Default HttpClient pointing at the app host (static assets, etc.)
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Named HttpClient for the BeHealthy API
builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("API:BaseUrl")!);
})
.AddStandardResilienceHandler();

builder.Services.AddLocalization();

builder.Services.AddFrontendServices();

var host = builder.Build();

// Apply the culture persisted in the browser (defaults to en-US).
const string defaultCulture = "en-US";
var js = host.Services.GetRequiredService<IJSRuntime>();
var storedCulture = await js.InvokeAsync<string?>("localStorage.getItem", "BlazorCulture");
var culture = new CultureInfo(string.IsNullOrWhiteSpace(storedCulture) ? defaultCulture : storedCulture);
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

await host.RunAsync();
