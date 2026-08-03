using BeHealthy.Frontend.Services.Interfaces;
using BeHealthy.Shared.Dtos.Dashboard;

namespace BeHealthy.Frontend.Services.Api;

public class DashboardApiService : ApiClientBase, IDashboardService
{
    public DashboardApiService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

    public async Task<DashboardSummaryResponse> GetSummaryAsync()
        => await GetAsync<DashboardSummaryResponse>("dashboard/summary") ?? new();
}
