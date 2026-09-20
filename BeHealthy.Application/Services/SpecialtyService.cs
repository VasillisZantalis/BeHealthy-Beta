using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.Specialty;
using BeHealthy.Shared.Locales;

namespace BeHealthy.Application.Services;

public class SpecialtyService : ISpecialtyService
{
    private readonly IUnitOfWork _unitOfWork;

    public SpecialtyService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<SpecialtyResponse>> GetSpecialtiesAsync()
    {
        var specialties = await _unitOfWork.SpecialtyRepository.GetAllAsync();
        return specialties.MapToDto();
    }

    public async Task<SpecialtyResponse?> GetSpecialtyByIdAsync(int id)
    {
        var specialty = await _unitOfWork.SpecialtyRepository.GetByIdAsync(id);
        return specialty?.MapToDto();
    }

    public async Task AddSpecialtyAsync(SpecialtyCreateRequest specialtyForCreationDto)
    {
        var specialty = specialtyForCreationDto.MapToDomain();
        await _unitOfWork.SpecialtyRepository.AddAsync(specialty);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<ServiceResponse> UpdateSpecialtyAsync(SpecialtyUpdateRequest specialtyForUpdateDto)
    {
        var specialty = await _unitOfWork.SpecialtyRepository.GetByIdAsync(specialtyForUpdateDto.Id);
        if (specialty is null)
            return ServiceResponse.Failed(Resource.NotFound);

        specialty.Name = specialtyForUpdateDto.Name;

        await _unitOfWork.SpecialtyRepository.UpdateAsync(specialty);
        await _unitOfWork.SaveChangesAsync();
        return ServiceResponse.Successful();
    }

    public async Task DeleteSpecialtyAsync(int id)
    {
        await _unitOfWork.SpecialtyRepository.DeleteAsync(id);
    }
}
