using BeHealthy.Shared.Dtos.Allergy;
using BeHealthy.Application.Interfaces;
using BeHealthy.Application.Mappings;
using BeHealthy.Shared.Locales;

namespace BeHealthy.Application.Services;

public class AllergyService : IAllergyService
{
    private readonly IUnitOfWork _unitOfWork;

    public AllergyService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<AllergyResponse>> GetAllergiesByPatientIdAsync(int patientId)
    {
        var allergies = await _unitOfWork.AllergyRepository.GetAllergiesByPatientIdAsync(patientId);
        return allergies.Select(a => a.MapToDto());
    }

    public async Task<ServiceResponse> AddAllergyAsync(AllergyCreateRequest dto)
    {
        var allergy = dto.MapToDomain();
        await _unitOfWork.AllergyRepository.AddAsync(allergy);
        await _unitOfWork.SaveChangesAsync();
        return ServiceResponse.Successful();
    }

    public async Task<ServiceResponse> UpdateAllergyAsync(AllergyUpdateRequest dto)
    {
        var allergy = await _unitOfWork.AllergyRepository.GetByIdAsync(dto.Id);
        if (allergy == null)
            return ServiceResponse.Failed(Resource.NotFound);

        allergy.AllergyName = dto.AllergyName;
        allergy.Allergen = dto.Allergen;
        allergy.Severity = dto.Severity;
        allergy.Notes = dto.Notes;
        allergy.PatientId = dto.PatientId;

        await _unitOfWork.AllergyRepository.UpdateAsync(allergy);
        await _unitOfWork.SaveChangesAsync();
        return ServiceResponse.Successful();
    }

    public async Task<ServiceResponse> DeleteAllergyAsync(int id)
    {
        await _unitOfWork.AllergyRepository.DeleteAsync(id);
        return ServiceResponse.Successful();
    }

    public async Task<AllergyResponse?> GetAllergyByIdAsync(int id)
    {
        var allergy = await _unitOfWork.AllergyRepository.GetByIdAsync(id);
        return allergy?.MapToDto();
    }
}
