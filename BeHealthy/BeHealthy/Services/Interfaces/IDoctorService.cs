using BeHealthy.Shared.Models.Dtos.Appointment;
using BeHealthy.Shared.Models.Dtos.Doctor;
using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Services.Interfaces;

public interface IDoctorService
{
    Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync();
    Task<DoctorDto> GetDoctorByIdAsync(int id);
    Task<IEnumerable<AppointmentDto>> GetDoctorAppointmentsByUserIdAsync(string userId);
    Task AddDoctorAsync(DoctorForCreationDto doctor);
    Task UpdateDoctorAsync(DoctorForUpdateDto doctor);
    Task DeleteDoctorAsync(int id);
}
