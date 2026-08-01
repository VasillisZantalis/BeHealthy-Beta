using BeHealthy.Shared.Dtos.Specialty;

namespace BeHealthy.Frontend.Services.Interfaces;

public interface ISpecialtyService
{
    Task<IEnumerable<SpecialtyDto>> GetSpecialtiesAsync();
    Task<SpecialtyDto?> GetSpecialtyByIdAsync(int id);
    Task AddSpecialtyAsync(SpecialtyCreateDto specialtyForCreationDto);
    Task UpdateSpecialtyAsync(SpecialtyUpdateDto specialtyForUpdateDto);
    Task DeleteSpecialtyAsync(int id);
}
