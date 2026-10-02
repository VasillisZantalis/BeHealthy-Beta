using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.Specialty;
using BeHealthy.Shared.Locales;

namespace BeHealthy.Application.Services;

public class SpecialtyService : ISpecialtyService
{
    private readonly ISpecialtyRepository _specialtyRepository;

    public SpecialtyService(ISpecialtyRepository specialtyRepository)
    {
        _specialtyRepository = specialtyRepository;
    }

    public async Task<IEnumerable<SpecialtyResponse>> GetSpecialtiesAsync(CancellationToken cancellationToken = default)
    {
        var specialties = await _specialtyRepository.GetAllAsync(cancellationToken);
        return specialties.MapToDto();
    }

    public async Task<SpecialtyResponse?> GetSpecialtyByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var specialty = await _specialtyRepository.GetByIdAsync(id, cancellationToken);
        return specialty?.MapToDto();
    }

    public async Task AddSpecialtyAsync(SpecialtyCreateRequest specialtyForCreationDto, CancellationToken cancellationToken = default)
    {
        var specialty = specialtyForCreationDto.MapToDomain();
        await _specialtyRepository.AddAsync(specialty, cancellationToken);
        await _specialtyRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task<ServiceResponse> UpdateSpecialtyAsync(SpecialtyUpdateRequest specialtyForUpdateDto, CancellationToken cancellationToken = default)
    {
        var specialty = await _specialtyRepository.GetByIdAsync(specialtyForUpdateDto.Id, cancellationToken);
        if (specialty is null)
        {
            return ServiceResponse.Failed(Resource.NotFound);
        }

        specialty.Name = specialtyForUpdateDto.Name;

        await _specialtyRepository.UpdateAsync(specialty);
        await _specialtyRepository.SaveChangesAsync(cancellationToken);
        return ServiceResponse.Successful();
    }

    public async Task DeleteSpecialtyAsync(int id, CancellationToken cancellationToken = default)
    {
        if (await _specialtyRepository.DeleteAsync(id, cancellationToken))
        {
            await _specialtyRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
