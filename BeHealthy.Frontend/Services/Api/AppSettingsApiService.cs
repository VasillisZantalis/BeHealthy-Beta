using System.Net.Http.Json;
using BeHealthy.Frontend.Services.Interfaces;
using BeHealthy.Shared.Dtos.Common;

namespace BeHealthy.Frontend.Services.Api;

public class AppSettingsApiService : ApiClientBase, IAppSettingsService
{
    public AppSettingsApiService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

    public async Task<IEnumerable<AppSettingResponse>> GetAppSettingsAsync()
        => await GetListAsync<AppSettingResponse>("appsettings");

    public async Task<List<AppSettingResponse>> GetMassAppSettingsAsync(List<string> keys)
        => await GetAsync<List<AppSettingResponse>>("appsettings/mass") is { } list ? list : await PostMassAsync(keys);

    private async Task<List<AppSettingResponse>> PostMassAsync(List<string> keys)
    {
        var response = await Http.PostAsJsonAsync("appsettings/mass", keys);
        if (!response.IsSuccessStatusCode)
            return new();
        return await response.Content.ReadFromJsonAsync<List<AppSettingResponse>>() ?? new();
    }

    public async Task<AppSettingResponse?> GetSettingByKeyAsync(string key)
        => await GetAsync<AppSettingResponse>($"appsettings/{Uri.EscapeDataString(key)}");

    public async Task UpdateSettingAsync(AppSettingUpdateRequest setting)
        => await PutAsync("appsettings", setting);
}
