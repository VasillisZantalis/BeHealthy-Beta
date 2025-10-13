namespace BeHealthy.Application.Services.Interfaces;

public interface IPatientService
{
    Task<IEnumerable<PatientDto>> GetAllPatientsAsync();
    Task<PatientDto?> GetPatientByIdAsync(int id);
    Task<IEnumerable<AppointmentDto>> GetPatientAppointmentsByUserIdAsync(string userId);
    Task<IEnumerable<PatientSimpleDto>> GetAllPatientsSimpleAsync();
    Task<IEnumerable<DoctorDto>> GetMyDoctorsAsync(string userId);
    Task<ServiceResponse> AddPatientAsync(PatientCreateDto patient);
    Task<int> GetPatientCountAsync();
    Task<ServiceResponse> UpdatePatientAsync(PatientUpdateDto patient);
    Task DeletePatientAsync(int id);
}
