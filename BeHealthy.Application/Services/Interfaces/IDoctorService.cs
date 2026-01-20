using BeHealthy.Shared.Parameters;

namespace BeHealthy.Application.Services.Interfaces;

public interface IDoctorService
{
    Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync(DoctorQueryParameters? parameters = null);
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
