using BeHealthy.Shared.Parameters;

namespace BeHealthy.Application.Services.Interfaces;

public interface IDoctorService
{
    Task<PaginatedResult<DoctorResponse>> GetAllDoctorsAsync(DoctorQueryParameters? parameters = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<DoctorSimpleResponse>> GetAllDoctorsSimpleAsync(CancellationToken cancellationToken = default);
    Task<DoctorResponse?> GetDoctorByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<PatientResponse>> GetMyPatientsAsync(string userId, CancellationToken cancellationToken = default);
    Task<ProfileResponse?> GetDoctorProfileByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AppointmentResponse>> GetDoctorAppointmentsByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<ServiceResponse> AddDoctorAsync(DoctorCreateRequest doctor, CancellationToken cancellationToken = default);
    Task<int> GetDoctorCountAsync(CancellationToken cancellationToken = default);
    Task<ServiceResponse> UpdateDoctorAsync(DoctorUpdateRequest doctor, CancellationToken cancellationToken = default);
    Task DeleteDoctorAsync(int id, CancellationToken cancellationToken = default);
}
