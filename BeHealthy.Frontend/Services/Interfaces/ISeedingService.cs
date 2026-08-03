using BeHealthy.Shared.Dtos.Common;

namespace BeHealthy.Frontend.Services.Interfaces;

public interface ISeedingService
{
    Task<Dictionary<string, int>> CheckEntityCountsAsync();
    Task<bool> NeedsSeedingAsync();
    Task<ServiceResponse> SeedDoctorsAsync(int count);
    Task<ServiceResponse> SeedPatientsAsync(int count);
    Task<ServiceResponse> SeedNursesAsync(int count);
    Task<ServiceResponse> SeedAppointmentsAsync(int count);
    Task<ServiceResponse> SeedAllAsync(SeedingOptionsRequest options);
}
