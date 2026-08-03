using BeHealthy.Shared.Dtos.Appointment;
using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.Doctor;
using BeHealthy.Shared.Dtos.Patient;
using BeHealthy.Shared.Dtos.User;
using BeHealthy.Shared.Parameters;

namespace BeHealthy.Frontend.Services.Interfaces;

public interface IDoctorService
{
    Task<PaginatedResult<DoctorResponse>> GetAllDoctorsAsync(DoctorQueryParameters? parameters = null);
    Task<IEnumerable<DoctorSimpleResponse>> GetAllDoctorsSimpleAsync();
    Task<DoctorResponse?> GetDoctorByIdAsync(int id);
    Task<IEnumerable<PatientResponse>> GetMyPatientsAsync(string userId);
    Task<ProfileResponse?> GetDoctorProfileByUserIdAsync(string userId);
    Task<IEnumerable<AppointmentResponse>> GetDoctorAppointmentsByUserIdAsync(string userId);
    Task<ServiceResponse> AddDoctorAsync(DoctorCreateRequest doctor);
    Task<int> GetDoctorCountAsync();
    Task<ServiceResponse> UpdateDoctorAsync(DoctorUpdateRequest doctor);
    Task DeleteDoctorAsync(int id);
}
