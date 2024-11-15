using BeHealthy.Application.Dtos.Appointment;
using BeHealthy.Application.Dtos.Patient;

namespace BeHealthy.Application.Services.Interfaces;

public interface IPatientService
{
    Task<IEnumerable<PatientDto>> GetAllPatientsAsync();
    Task<PatientDto> GetPatientByIdAsync(int id);
    Task<IEnumerable<AppointmentDto>> GetPatientAppointmentsByUserIdAsync(string userId);
    Task AddPatientAsync(PatientForCreationDto patient);
    Task UpdatePatientAsync(int id, PatientForUpdateDto patient);
    Task DeletePatientAsync(int id);
}
