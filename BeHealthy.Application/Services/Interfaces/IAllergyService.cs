using BeHealthy.Application.Dtos.Allergy;

namespace BeHealthy.Application.Services.Interfaces;

public interface IAllergyService
{
    Task<IEnumerable<AllergyDto>> GetAllergiesByPatientIdAsync(int patientId);
    Task<AllergyDto?> GetAllergyByIdAsync(int id);
    Task<ServiceResponse> AddAllergyAsync(AllergyCreateDto dto);
    Task<ServiceResponse> UpdateAllergyAsync(AllergyUpdateDto dto);
    Task<ServiceResponse> DeleteAllergyAsync(int id);
}