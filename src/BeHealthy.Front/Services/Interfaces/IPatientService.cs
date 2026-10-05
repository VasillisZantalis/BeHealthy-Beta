using BeHealthy.Shared.Dtos.Appointment;
using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.Doctor;
using BeHealthy.Shared.Dtos.Patient;
using BeHealthy.Shared.Dtos.User;
using BeHealthy.Shared.Parameters;

namespace BeHealthy.Front.Services.Interfaces;

public interface IPatientService
{
    Task<IEnumerable<PatientResponse>> GetAllPatientsAsync(PatientQueryParameters? parameters = null);
    Task<PatientResponse?> GetPatientByIdAsync(int id);
    Task<IEnumerable<PatientSimpleResponse>> GetAllPatientsSimpleAsync();
    Task<IEnumerable<DoctorResponse>> GetMyDoctorsAsync();
    Task<ServiceResponse> AddPatientAsync(PatientCreateRequest patient);
    Task<int> GetPatientCountAsync();
    Task<ServiceResponse> UpdatePatientAsync(PatientUpdateRequest patient);
    Task DeletePatientAsync(int id);
}
