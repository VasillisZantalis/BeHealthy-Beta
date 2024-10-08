using BeHealthy.Shared.Models.Dtos.Patient;

namespace BeHealthy.Client.Services.Interfaces;

public interface IPatientService
{
    Task<IEnumerable<PatientDto>> GetAllPatientsAsync();
    Task<PatientDto>? GetPatientByIdAsync(int id);
    Task AddPatientAsync(PatientForCreationDto patientDto);
    Task UpdatePatientAsync(int id, PatientForUpdateDto patientDto);
    Task DeletePatientAsync(int id);
}
