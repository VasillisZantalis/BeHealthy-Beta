using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.Specialty;

namespace BeHealthy.Application.Services.Interfaces;

public interface ISpecialtyService
{
    Task<IEnumerable<SpecialtyResponse>> GetSpecialtiesAsync(CancellationToken cancellationToken = default);
    Task<SpecialtyResponse?> GetSpecialtyByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddSpecialtyAsync(SpecialtyCreateRequest specialtyForCreationDto, CancellationToken cancellationToken = default);
    Task<ServiceResponse> UpdateSpecialtyAsync(SpecialtyUpdateRequest specialtyForUpdateDto, CancellationToken cancellationToken = default);
    Task<ServiceResponse> DeleteSpecialtyAsync(int id, CancellationToken cancellationToken = default);
}
