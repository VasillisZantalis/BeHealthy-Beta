namespace BeHealthy.Application.Services.Interfaces;

public interface IAppointmentService
{
    Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync();
    Task<IEnumerable<AppointmentDto>> GetAllAppointmentsByDoctorIdAsync(int doctorId);
    Task<IEnumerable<AppointmentDto>> GetAllAppointmentsByPatientIdAsync(int patientId);
    Task<IEnumerable<AppointmentDto>> GetAllAppointmentsByUserIdAsync(string userId);
    Task<Dictionary<AppointmentReason, int>> GetAppointmentReasonCounts();
    Task<AppointmentDto?> GetAppointmentByIdAsync(int id);
    Task<ServiceResponse> AddAppointmentAsync(AppointmentCreateDto appointment);
    Task<ServiceResponse> UpdateAppointmentAsync(AppointmentUpdateDto appointment);
    Task DeleteAppointmentAsync(int id);
}
