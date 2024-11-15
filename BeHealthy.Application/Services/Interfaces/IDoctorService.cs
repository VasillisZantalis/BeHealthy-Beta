using BeHealthy.Application.Dtos.Appointment;
using BeHealthy.Application.Dtos.Doctor;

namespace BeHealthy.Application.Services.Interfaces;

public interface IDoctorService
{
    Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync();
    Task<DoctorDto> GetDoctorByIdAsync(int id);
    Task<IEnumerable<AppointmentDto>> GetDoctorAppointmentsByUserIdAsync(string userId);
    Task AddDoctorAsync(DoctorForCreationDto doctor);
    Task UpdateDoctorAsync(int id, DoctorForUpdateDto doctor);
    Task DeleteDoctorAsync(int id);
}
