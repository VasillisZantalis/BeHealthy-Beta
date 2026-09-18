using BeHealthy.Shared.Dtos.Dashboard;

namespace BeHealthy.Front.Services.Interfaces;

public interface IDashboardService
{
    Task<DashboardSummaryResponse> GetSummaryAsync();
}
