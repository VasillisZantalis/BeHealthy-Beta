using System.Net.Http.Json;
using BeHealthy.Front.Common;
using BeHealthy.Front.Services.Interfaces;
using BeHealthy.Shared.Dtos.Common;

namespace BeHealthy.Front.Services.Api;

public class AppSettingsApiService : ApiClientBase, IAppSettingsService
{
    public AppSettingsApiService(IHttpClientFactory httpClientFactory, ICurrentUserService currentUser) : base(httpClientFactory, currentUser) { }

    public async Task<IEnumerable<AppSettingResponse>> GetAppSettingsAsync()
        => await GetListAsync<AppSettingResponse>("settings");

    public async Task<List<AppSettingResponse>> GetMassAppSettingsAsync(List<string> keys)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("settings/bulk", keys, ApiJsonOptions.Default);
            if (!response.IsSuccessStatusCode)
            {
                return new();
            }

            return await response.Content.ReadFromJsonAsync<List<AppSettingResponse>>(ApiJsonOptions.Default) ?? new();
        }
        catch (HttpRequestException)
        {
            return new();
        }
    }

    public async Task<AppSettingResponse?> GetSettingByKeyAsync(string key)
        => await GetAsync<AppSettingResponse>($"settings/{Uri.EscapeDataString(key)}");

    public async Task<ServiceResponse> UpdateSettingAsync(AppSettingUpdateRequest setting)
        => await PutForResponseAsync($"settings/{Uri.EscapeDataString(setting.Key)}", setting);
}
