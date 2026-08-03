using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.Prescription;

namespace BeHealthy.Application.Services.Interfaces;

public interface IPrescriptionService
{
    Task<IEnumerable<PrescriptionResponse>> GetAllPrescriptionsAsync();
    Task<PrescriptionResponse?> GetPrescriptionByIdAsync(int id);
    Task<IEnumerable<PrescriptionResponse>> GetPrescriptionsByPatientIdAsync(int id);
    Task<ServiceResponse> AddPrescriptionAsync(PrescriptionCreateRequest prescriptionDto);
    Task<ServiceResponse> UpdatePrescriptionAsync(PrescriptionUpdateRequest prescriptionDto);
    Task<ServiceResponse> DeletePrescriptionAsync(int id);
}
