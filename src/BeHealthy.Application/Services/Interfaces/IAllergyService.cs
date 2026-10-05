using BeHealthy.Shared.Dtos.Allergy;

namespace BeHealthy.Application.Services.Interfaces;

public interface IAllergyService
{
    Task<IEnumerable<AllergyResponse>> GetAllergiesByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
    Task<AllergyResponse?> GetAllergyByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ServiceResponse> AddAllergyAsync(AllergyCreateRequest dto, CancellationToken cancellationToken = default);
    Task<ServiceResponse> UpdateAllergyAsync(AllergyUpdateRequest dto, CancellationToken cancellationToken = default);
    Task<ServiceResponse> DeleteAllergyAsync(int id, CancellationToken cancellationToken = default);
}