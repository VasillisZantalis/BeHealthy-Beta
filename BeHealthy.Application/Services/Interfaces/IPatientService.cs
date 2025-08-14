using BeHealthy.Shared.Parameters;

namespace BeHealthy.Application.Services.Interfaces;

public interface IPatientService
{
    Task<IEnumerable<PatientDto>> GetAllPatientsAsync(PatientSearchingParameters patientSearchingParameters);
    Task<PatientDto?> GetPatientByIdAsync(int id);
    Task<IEnumerable<AppointmentDto>> GetPatientAppointmentsByUserIdAsync(string userId);
    Task<IEnumerable<DoctorDto>> GetMyDoctorsAsync(string userId);
    Task<ServiceResponse> AddPatientAsync(PatientForCreationDto patient);
    Task<int> GetPatientCountAsync();
    Task<ServiceResponse> UpdatePatientAsync(PatientForUpdateDto patient);
    Task DeletePatientAsync(int id);
}
