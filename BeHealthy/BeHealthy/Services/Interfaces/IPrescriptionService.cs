using BeHealthy.Shared.Models.Dtos;
using BeHealthy.Shared.Models.Dtos.Prescription;

namespace BeHealthy.Services.Interfaces;

public interface IPrescriptionService
{
    Task<IEnumerable<PrescriptionDto>> GetAllPrescriptionsAsync();
    Task<PrescriptionDto> GetPrescriptionByIdAsync(int id);
    Task<IEnumerable<PrescriptionDto>> GetPrescriptionsByPatientIdAsync(int id);
    Task AddPrescriptionAsync(PrescriptionForCreationDto prescriptionDto);
    Task UpdatePrescriptionAsync(PrescriptionForUpdateDto prescriptionDto);
    Task DeletePrescriptionAsync(int id);
}
