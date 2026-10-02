using BeHealthy.Shared.Dtos.Common;

namespace BeHealthy.Application.Services.Interfaces;

public interface ISeedingService
{
    Task<Dictionary<string, int>> CheckEntityCountsAsync(CancellationToken cancellationToken = default);
    Task<bool> NeedsSeedingAsync(CancellationToken cancellationToken = default);
    Task<ServiceResponse> SeedDoctorsAsync(int count, CancellationToken cancellationToken = default);
    Task<ServiceResponse> SeedPatientsAsync(int count, CancellationToken cancellationToken = default);
    Task<ServiceResponse> SeedNursesAsync(int count, CancellationToken cancellationToken = default);
    Task<ServiceResponse> SeedAppointmentsAsync(int count, CancellationToken cancellationToken = default);
    Task<ServiceResponse> SeedAllAsync(SeedingOptionsRequest options, CancellationToken cancellationToken = default);
}