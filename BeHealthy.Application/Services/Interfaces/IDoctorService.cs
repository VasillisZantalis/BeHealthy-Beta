using BeHealthy.Application.Dtos.Appointment;
using BeHealthy.Application.Dtos.Common;
using BeHealthy.Application.Dtos.Doctor;
using BeHealthy.Application.Dtos.Patient;
using BeHealthy.Application.Dtos.User;

namespace BeHealthy.Application.Services.Interfaces;

public interface IDoctorService
{
    Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync();
    Task<DoctorDto?> GetDoctorByIdAsync(int id);
    Task<IEnumerable<PatientDto>> GetMyPatientsAsync(string userId);
    Task<ProfileDto?> GetDoctorProfileByUserIdAsync(string userId);
    Task<IEnumerable<AppointmentDto>> GetDoctorAppointmentsByUserIdAsync(string userId);
    Task<ServiceResponse> AddDoctorAsync(DoctorForCreationDto doctor);
    Task<int> GetDoctorCountAsync();
    Task UpdateDoctorAsync(int id, DoctorForUpdateDto doctor);
    Task DeleteDoctorAsync(int id);
}
