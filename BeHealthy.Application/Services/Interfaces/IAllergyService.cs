using BeHealthy.Shared.Dtos.Allergy;

namespace BeHealthy.Application.Services.Interfaces;

public interface IAllergyService
{
    Task<IEnumerable<AllergyResponse>> GetAllergiesByPatientIdAsync(int patientId);
    Task<AllergyResponse?> GetAllergyByIdAsync(int id);
    Task<ServiceResponse> AddAllergyAsync(AllergyCreateRequest dto);
    Task<ServiceResponse> UpdateAllergyAsync(AllergyUpdateRequest dto);
    Task<ServiceResponse> DeleteAllergyAsync(int id);
}