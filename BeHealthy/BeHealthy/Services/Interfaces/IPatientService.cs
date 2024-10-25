using BeHealthy.Shared.Models.Dtos.Appointment;
using BeHealthy.Shared.Models.Dtos.Patient;

namespace BeHealthy.Services.Interfaces;

public interface IPatientService
{
    Task<IEnumerable<PatientDto>> GetAllPatientsAsync();
    Task<PatientDto> GetPatientByIdAsync(int id);
    Task<IEnumerable<AppointmentDto>> GetPatientAppointmentsByUserIdAsync(string userId);
    Task AddPatientAsync(PatientForCreationDto patient);
    Task UpdatePatientAsync(int id, PatientForUpdateDto patient);
    Task DeletePatientAsync(int id);
}
