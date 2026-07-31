using BeHealthy.Shared.Dtos.Appointment;
using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.Doctor;
using BeHealthy.Shared.Dtos.Patient;
using BeHealthy.Shared.Dtos.User;
using BeHealthy.Shared.Parameters;

namespace BeHealthy.Frontend.Services.Interfaces;

public interface IPatientService
{
    Task<IEnumerable<PatientDto>> GetAllPatientsAsync(PatientQueryParameters? parameters = null);
    Task<PatientDto?> GetPatientByIdAsync(int id);
    Task<IEnumerable<AppointmentDto>> GetPatientAppointmentsByUserIdAsync(string userId);
    Task<IEnumerable<PatientSimpleDto>> GetAllPatientsSimpleAsync();
    Task<ProfileDto?> GetPatientProfileByUserIdAsync(string userId);
    Task<IEnumerable<DoctorDto>> GetMyDoctorsAsync(string userId);
    Task<ServiceResponse> AddPatientAsync(PatientCreateDto patient);
    Task<int> GetPatientCountAsync();
    Task<ServiceResponse> UpdatePatientAsync(PatientUpdateDto patient);
    Task DeletePatientAsync(int id);
}
