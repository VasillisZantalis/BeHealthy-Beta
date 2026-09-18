using BeHealthy.Front.Services.Interfaces;
using BeHealthy.Shared.Dtos.Dashboard;

namespace BeHealthy.Front.Services.Api;

public class DashboardApiService : ApiClientBase, IDashboardService
{
    public DashboardApiService(IHttpClientFactory httpClientFactory, ICurrentUserService currentUser) : base(httpClientFactory, currentUser) { }

    public async Task<DashboardSummaryResponse> GetSummaryAsync()
        => await GetAsync<DashboardSummaryResponse>("dashboard/summary") ?? new();
}
