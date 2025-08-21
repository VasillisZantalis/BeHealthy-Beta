using BeHealthy.Application.Dtos.Allergy;

namespace BeHealthy.Application.Services.Interfaces;

public interface IAllergyService
{
    Task<IEnumerable<Allergy>> GetAllergiesByPatientIdAsync(int patientId);
    Task<ServiceResponse> AddAllergyAsync(AllergyCreateDto dto);
    Task<ServiceResponse> UpdateAllergyAsync(AllergyUpdateDto dto);
    Task<ServiceResponse> DeleteAllergyAsync(int id);
}