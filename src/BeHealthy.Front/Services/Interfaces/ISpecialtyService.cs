using BeHealthy.Shared.Dtos.Specialty;

namespace BeHealthy.Front.Services.Interfaces;

public interface ISpecialtyService
{
    Task<IEnumerable<SpecialtyResponse>> GetSpecialtiesAsync();
    Task<SpecialtyResponse?> GetSpecialtyByIdAsync(int id);
    Task AddSpecialtyAsync(SpecialtyCreateRequest specialtyForCreationDto);
    Task UpdateSpecialtyAsync(SpecialtyUpdateRequest specialtyForUpdateDto);
    Task DeleteSpecialtyAsync(int id);
}
