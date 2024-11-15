using BeHealthy.Application.Dtos.Prescription;

namespace BeHealthy.Application.Services.Interfaces;

public interface IPrescriptionService
{
    Task<IEnumerable<PrescriptionDto>> GetAllPrescriptionsAsync();
    Task<PrescriptionDto> GetPrescriptionByIdAsync(int id);
    Task<IEnumerable<PrescriptionDto>> GetPrescriptionsByPatientIdAsync(int id);
    Task AddPrescriptionAsync(PrescriptionForCreationDto prescriptionDto);
    Task UpdatePrescriptionAsync(PrescriptionForUpdateDto prescriptionDto);
    Task DeletePrescriptionAsync(int id);
}
