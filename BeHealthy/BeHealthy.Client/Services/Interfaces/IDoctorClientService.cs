using BeHealthy.Shared.Models.Dtos.Appointment;
using BeHealthy.Shared.Models.Dtos.Doctor;

namespace BeHealthy.Client.Services.Interfaces;

public interface IDoctorClientService
{
    Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync();
    Task<DoctorDto>? GetDoctorByIdAsync(int id);
    Task<IEnumerable<AppointmentDto>> GetDoctorAppointmentsByUserIdAsync(string userId);
    Task AddDoctorAsync(DoctorForCreationDto doctorDto);
    Task UpdateDoctorAsync(int id, DoctorForUpdateDto doctorDto);
    Task DeleteDoctorAsync(int id);
}
