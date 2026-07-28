using BeHealthy.Shared.Dtos.Allergy;
using BeHealthy.Application.Interfaces.Repositories;
using BeHealthy.Application.Mappings;
using BeHealthy.Shared.Locales;

namespace BeHealthy.Application.Services;

public class AllergyService : IAllergyService
{
    private readonly IAllergyRepository _allergyRepository;

    public AllergyService(IAllergyRepository allergyRepository)
    {
        _allergyRepository = allergyRepository;
    }

    public async Task<IEnumerable<AllergyDto>> GetAllergiesByPatientIdAsync(int patientId)
    {
        var allergies = await _allergyRepository.GetAllergiesByPatientIdAsync(patientId);
        return allergies.Select(a => a.MapToDto());
    }

    public async Task<ServiceResponse> AddAllergyAsync(AllergyCreateDto dto)
    {
        var allergy = dto.MapToDomain();
        await _allergyRepository.AddAsync(allergy);
        return ServiceResponse.Successful();
    }

    public async Task<ServiceResponse> UpdateAllergyAsync(AllergyUpdateDto dto)
    {
        var allergy = await _allergyRepository.GetByIdAsync(dto.Id);
        if (allergy == null)
            return ServiceResponse.Failed(Resource.NotFound);

        allergy = dto.MapToDomain();
        await _allergyRepository.UpdateAsync(allergy);
        return ServiceResponse.Successful();
    }

    public async Task<ServiceResponse> DeleteAllergyAsync(int id)
    {
        await _allergyRepository.DeleteAsync(id);
        return ServiceResponse.Successful();
    }

    public async Task<AllergyDto?> GetAllergyByIdAsync(int id)
    {
        var allergy = await _allergyRepository.GetByIdAsync(id);
        return allergy?.MapToDto();
    }
}