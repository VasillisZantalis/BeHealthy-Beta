using BeHealthy.Domain;

namespace BeHealthy.Application.Dtos.Appointment;

public class AppointmentCreateDto
{
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public int? RoomId { get; set; }
    public int? NurseId { get; set; }
    public DateOnly AppointmentDate { get; set; }
    public TimeOnly AppointmentStartTime { get; set; }
    public TimeOnly AppointmentEndTime { get; set; }
    public AppointmentReason Reason { get; set; }
    public AppointmentStatus Status { get; set; }
    public string Notes { get; set; } = string.Empty;
}
