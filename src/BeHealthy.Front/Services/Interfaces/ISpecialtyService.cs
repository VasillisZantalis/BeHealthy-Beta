using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.Specialty;

namespace BeHealthy.Front.Services.Interfaces;

public interface ISpecialtyService
{
    Task<IEnumerable<SpecialtyResponse>> GetSpecialtiesAsync();
    Task<SpecialtyResponse?> GetSpecialtyByIdAsync(int id);
    Task<ServiceResponse> AddSpecialtyAsync(SpecialtyCreateRequest specialtyForCreationDto);
    Task<ServiceResponse> UpdateSpecialtyAsync(SpecialtyUpdateRequest specialtyForUpdateDto);
    Task DeleteSpecialtyAsync(int id);
}
