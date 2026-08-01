using BeHealthy.Shared.Dtos.Dashboard;

namespace BeHealthy.Frontend.Services.Interfaces;

public interface IDashboardService
{
    Task<DashboardSummaryDto> GetSummaryAsync();
}
