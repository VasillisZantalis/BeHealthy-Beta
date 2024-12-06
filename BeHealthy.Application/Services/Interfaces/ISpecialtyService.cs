using BeHealthy.Application.Dtos.Specialty;

namespace BeHealthy.Application.Services.Interfaces;

public interface ISpecialtyService
{
    Task<IEnumerable<SpecialtyDto>> GetSpecialtiesAsync();
    Task<SpecialtyDto> GetSpecialtyByIdAsync(int id);
    Task AddSpecialtyAsync(SpecialtyForCreationDto specialtyForCreationDto);
    Task UpdateSpecialtyAsync(int id, SpecialtyForUpdateDto specialtyForUpdateDto);
    Task DeleteSpecialtyAsync(int id);
}
