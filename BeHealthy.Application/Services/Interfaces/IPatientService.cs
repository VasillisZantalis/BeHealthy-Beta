using BeHealthy.Application.Dtos.Appointment;
using BeHealthy.Application.Dtos.Common;
using BeHealthy.Application.Dtos.Doctor;
using BeHealthy.Application.Dtos.Patient;
using BeHealthy.Shared.Parameters;

namespace BeHealthy.Application.Services.Interfaces;

public interface IPatientService
{
    Task<IEnumerable<PatientDto>> GetAllPatientsAsync(PatientSearchingParameters patientSearchingParameters);
    Task<PatientDto?> GetPatientByIdAsync(int id);
    Task<IEnumerable<AppointmentDto>> GetPatientAppointmentsByUserIdAsync(string userId);
    Task<IEnumerable<DoctorDto>> GetMyDoctorsAsync(string userId);
    Task<ServiceResponse> AddPatientAsync(PatientForCreationDto patient);
    Task UpdatePatientAsync(int id, PatientForUpdateDto patient);
    Task DeletePatientAsync(int id);
}
