using BeHealthy.Application.Dtos.Specialty;
using BeHealthy.Application.Interfaces;
using BeHealthy.Application.Mappings;
using BeHealthy.Application.Services.Interfaces;

namespace BeHealthy.Application.Services;

public class SpecialtyService : ISpecialtyService
{
    private readonly IUnitOfWork _unitOfWork;

    public SpecialtyService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<SpecialtyDto>> GetSpecialtiesAsync()
    {
        var specialties = await _unitOfWork.SpecialtyRepository.GetAllAsync();
        return specialties.MapToDto();
    }

    public async Task<SpecialtyDto?> GetSpecialtyByIdAsync(int id)
    {
        var specialty = await _unitOfWork.SpecialtyRepository.GetByIdAsync(id);
        return specialty?.MapToDto();
    }

    public async Task AddSpecialtyAsync(SpecialtyCreateDto specialtyForCreationDto)
    {
        var specialty = specialtyForCreationDto.MapToDomain();
        await _unitOfWork.SpecialtyRepository.AddAsync(specialty);
    }

    public async Task UpdateSpecialtyAsync(int id, SpecialtyUpdateDto specialtyForUpdateDto)
    {
        var specialty = specialtyForUpdateDto.MapToDomain();

        if (await _unitOfWork.SpecialtyRepository.ExistsAsync(specialtyForUpdateDto.Id))
            return;

        await _unitOfWork.SpecialtyRepository.UpdateAsync(specialty);
    }

    public async Task DeleteSpecialtyAsync(int id)
    {
        await _unitOfWork.SpecialtyRepository.DeleteAsync(id);
    }
}
