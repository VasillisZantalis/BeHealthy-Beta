using BeHealthy.Shared.Dtos.Allergy;
using BeHealthy.Application.Interfaces;
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

    public async Task<IEnumerable<AllergyResponse>> GetAllergiesByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        var allergies = await _allergyRepository.GetAllergiesByPatientIdAsync(patientId, cancellationToken);
        return allergies.Select(a => a.MapToDto());
    }

    public async Task<ServiceResponse> AddAllergyAsync(AllergyCreateRequest dto, CancellationToken cancellationToken = default)
    {
        var allergy = dto.MapToDomain();
        await _allergyRepository.AddAsync(allergy, cancellationToken);
        await _allergyRepository.SaveChangesAsync(cancellationToken);
        return ServiceResponse.Successful();
    }

    public async Task<ServiceResponse> UpdateAllergyAsync(AllergyUpdateRequest dto, CancellationToken cancellationToken = default)
    {
        var allergy = await _allergyRepository.GetByIdAsync(dto.Id, cancellationToken);
        if (allergy == null)
            return ServiceResponse.Failed(Resource.NotFound);

        allergy.AllergyName = dto.AllergyName;
        allergy.Allergen = dto.Allergen;
        allergy.Severity = dto.Severity;
        allergy.Notes = dto.Notes;
        allergy.PatientId = dto.PatientId;

        await _allergyRepository.UpdateAsync(allergy);
        await _allergyRepository.SaveChangesAsync(cancellationToken);
        return ServiceResponse.Successful();
    }

    public async Task<ServiceResponse> DeleteAllergyAsync(int id, CancellationToken cancellationToken = default)
    {
        if (!await _allergyRepository.DeleteAsync(id, cancellationToken))
            return ServiceResponse.Failed(Resource.NotFound);

        await _allergyRepository.SaveChangesAsync(cancellationToken);
        return ServiceResponse.Successful();
    }

    public async Task<AllergyResponse?> GetAllergyByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var allergy = await _allergyRepository.GetByIdAsync(id, cancellationToken);
        return allergy?.MapToDto();
    }
}
