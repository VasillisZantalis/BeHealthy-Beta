using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.Prescription;

namespace BeHealthy.Application.Services.Interfaces;

public interface IPrescriptionService
{
    Task<IEnumerable<PrescriptionDto>> GetAllPrescriptionsAsync();
    Task<PrescriptionDto?> GetPrescriptionByIdAsync(int id);
    Task<IEnumerable<PrescriptionDto>> GetPrescriptionsByPatientIdAsync(int id);
    Task<ServiceResponse> AddPrescriptionAsync(PrescriptionCreateDto prescriptionDto);
    Task<ServiceResponse> UpdatePrescriptionAsync(PrescriptionUpdateDto prescriptionDto);
    Task<ServiceResponse> DeletePrescriptionAsync(int id);
}
