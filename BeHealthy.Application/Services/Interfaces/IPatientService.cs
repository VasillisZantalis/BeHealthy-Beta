using BeHealthy.Shared.Parameters;

namespace BeHealthy.Application.Services.Interfaces;

public interface IPatientService
{
    Task<IEnumerable<PatientResponse>> GetAllPatientsAsync(PatientQueryParameters? parameters = null);
    Task<PatientResponse?> GetPatientByIdAsync(int id);
    Task<IEnumerable<AppointmentResponse>> GetPatientAppointmentsByUserIdAsync(string userId);
    Task<IEnumerable<PatientSimpleResponse>> GetAllPatientsSimpleAsync();
    Task<ProfileResponse?> GetPatientProfileByUserIdAsync(string userId);
    Task<IEnumerable<DoctorResponse>> GetMyDoctorsAsync(string userId);
    Task<ServiceResponse> AddPatientAsync(PatientCreateRequest patient);
    Task<int> GetPatientCountAsync();
    Task<ServiceResponse> UpdatePatientAsync(PatientUpdateRequest patient);
    Task DeletePatientAsync(int id);
}
