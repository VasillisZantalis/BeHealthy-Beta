using System.Net.Http.Json;
using BeHealthy.Frontend.Services.Interfaces;
using BeHealthy.Shared.Dtos.Common;

namespace BeHealthy.Frontend.Services.Api;

public class AppSettingsApiService : ApiClientBase, IAppSettingsService
{
    public AppSettingsApiService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

    public async Task<IEnumerable<AppSettingDto>> GetAppSettingsAsync()
        => await GetListAsync<AppSettingDto>("appsettings");

    public async Task<List<AppSettingDto>> GetMassAppSettingsAsync(List<string> keys)
        => await GetAsync<List<AppSettingDto>>("appsettings/mass") is { } list ? list : await PostMassAsync(keys);

    private async Task<List<AppSettingDto>> PostMassAsync(List<string> keys)
    {
        var response = await Http.PostAsJsonAsync("appsettings/mass", keys);
        if (!response.IsSuccessStatusCode)
            return new();
        return await response.Content.ReadFromJsonAsync<List<AppSettingDto>>() ?? new();
    }

    public async Task<AppSettingDto?> GetSettingByKeyAsync(string key)
        => await GetAsync<AppSettingDto>($"appsettings/{Uri.EscapeDataString(key)}");

    public async Task UpdateSettingAsync(AppSettingUpdateDto setting)
        => await PutAsync("appsettings", setting);
}
