using BeHealthy.Shared.Models.Dtos;
using BeHealthy.Shared.Models.Dtos.Prescription;

namespace BeHealthy.Shared.Interfaces;

public interface IPrescriptionService
{
    Task<IEnumerable<PrescriptionDto>> GetAllPrescriptionsAsync();
    Task<PrescriptionDto> GetPrescriptionByIdAsync(int id);
    Task AddPrescriptionAsync(PrescriptionForCreationDto prescriptionDto);
    Task UpdatePrescriptionAsync(PrescriptionForUpdateDto prescriptionDto);
    Task DeletePrescriptionAsync(int id);
}
