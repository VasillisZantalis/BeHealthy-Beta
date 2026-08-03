using BeHealthy.Frontend.Services.Interfaces;
using BeHealthy.Shared.Dtos.Common;

namespace BeHealthy.Frontend.Services.Api;

public class SeedingApiService : ApiClientBase, ISeedingService
{
    public SeedingApiService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

    public async Task<Dictionary<string, int>> CheckEntityCountsAsync()
        => await GetAsync<Dictionary<string, int>>("seeding/counts") ?? new();

    public async Task<bool> NeedsSeedingAsync()
        => await GetAsync<bool>("seeding/needs-seeding");

    public async Task<ServiceResponse> SeedDoctorsAsync(int count)
        => await PostForResponseAsync($"seeding/doctors?count={count}", new { });

    public async Task<ServiceResponse> SeedPatientsAsync(int count)
        => await PostForResponseAsync($"seeding/patients?count={count}", new { });

    public async Task<ServiceResponse> SeedNursesAsync(int count)
        => await PostForResponseAsync($"seeding/nurses?count={count}", new { });

    public async Task<ServiceResponse> SeedAppointmentsAsync(int count)
        => await PostForResponseAsync($"seeding/appointments?count={count}", new { });

    public async Task<ServiceResponse> SeedAllAsync(SeedingOptionsRequest options)
        => await PostForResponseAsync("seeding/all", options);
}
