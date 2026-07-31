using BeHealthy.Shared.Dtos.Appointment;
using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.Doctor;
using BeHealthy.Shared.Dtos.Patient;
using BeHealthy.Shared.Dtos.User;
using BeHealthy.Shared.Parameters;

namespace BeHealthy.Frontend.Services.Interfaces;

public interface IDoctorService
{
    Task<PaginatedResult<DoctorDto>> GetAllDoctorsAsync(DoctorQueryParameters? parameters = null);
    Task<IEnumerable<DoctorSimpleDto>> GetAllDoctorsSimpleAsync();
    Task<DoctorDto?> GetDoctorByIdAsync(int id);
    Task<IEnumerable<PatientDto>> GetMyPatientsAsync(string userId);
    Task<ProfileDto?> GetDoctorProfileByUserIdAsync(string userId);
    Task<IEnumerable<AppointmentDto>> GetDoctorAppointmentsByUserIdAsync(string userId);
    Task<ServiceResponse> AddDoctorAsync(DoctorCreateDto doctor);
    Task<int> GetDoctorCountAsync();
    Task<ServiceResponse> UpdateDoctorAsync(DoctorUpdateDto doctor);
    Task DeleteDoctorAsync(int id);
}
