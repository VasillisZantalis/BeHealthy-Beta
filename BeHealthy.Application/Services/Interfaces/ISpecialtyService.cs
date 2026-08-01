using BeHealthy.Shared.Dtos.Specialty;

namespace BeHealthy.Application.Services.Interfaces;

public interface ISpecialtyService
{
    Task<IEnumerable<SpecialtyDto>> GetSpecialtiesAsync();
    Task<SpecialtyDto?> GetSpecialtyByIdAsync(int id);
    Task AddSpecialtyAsync(SpecialtyCreateDto specialtyForCreationDto);
    Task UpdateSpecialtyAsync(SpecialtyUpdateDto specialtyForUpdateDto);
    Task DeleteSpecialtyAsync(int id);
}
