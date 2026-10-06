using BeHealthy.Shared.Parameters;

namespace BeHealthy.Application.Services.Interfaces;

public interface IPatientService
{
    Task<IEnumerable<PatientResponse>> GetAllPatientsAsync(PatientQueryParameters? parameters = null, CancellationToken cancellationToken = default);
    Task<PatientResponse?> GetPatientByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<PatientSimpleResponse>> GetAllPatientsSimpleAsync(CancellationToken cancellationToken = default);
    Task<ProfileResponse?> GetPatientProfileByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<DoctorResponse>> GetMyDoctorsAsync(string userId, CancellationToken cancellationToken = default);
    Task<ServiceResponse> AddPatientAsync(PatientCreateRequest patient, CancellationToken cancellationToken = default);
    Task<int> GetPatientCountAsync(CancellationToken cancellationToken = default);
    Task<ServiceResponse> UpdatePatientAsync(PatientUpdateRequest patient, CancellationToken cancellationToken = default);
    Task<ServiceResponse> DeletePatientAsync(int id, CancellationToken cancellationToken = default);
}
