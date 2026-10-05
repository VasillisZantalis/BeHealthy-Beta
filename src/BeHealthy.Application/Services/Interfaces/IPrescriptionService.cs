using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.Prescription;

namespace BeHealthy.Application.Services.Interfaces;

public interface IPrescriptionService
{
    Task<IEnumerable<PrescriptionResponse>> GetAllPrescriptionsAsync(CancellationToken cancellationToken = default);
    Task<PrescriptionResponse?> GetPrescriptionByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<PrescriptionResponse>> GetPrescriptionsByPatientIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ServiceResponse> AddPrescriptionAsync(PrescriptionCreateRequest prescriptionDto, CancellationToken cancellationToken = default);
    Task<ServiceResponse> UpdatePrescriptionAsync(PrescriptionUpdateRequest prescriptionDto, CancellationToken cancellationToken = default);
    Task<ServiceResponse> DeletePrescriptionAsync(int id, CancellationToken cancellationToken = default);
}
