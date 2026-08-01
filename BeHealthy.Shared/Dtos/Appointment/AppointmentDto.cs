using BeHealthy.Shared.Dtos.Doctor;
using BeHealthy.Shared.Dtos.Nurse;
using BeHealthy.Shared.Dtos.Patient;
using BeHealthy.Shared.Dtos.Room;

namespace BeHealthy.Shared.Dtos.Appointment;

public class AppointmentDto
{
    public int Id { get; set; }
    public DateOnly AppointmentDate { get; set; }
    public TimeOnly AppointmentStartTime { get; set; }
    public TimeOnly AppointmentEndTime { get; set; }
    public string Notes { get; set; } = string.Empty;
    public AppointmentStatus Status { get; set; }
    public AppointmentReason Reason { get; set; }
    public int PatientId { get; set; }
    public PatientDto? Patient { get; set; }
    public int DoctorId { get; set; }
    public DoctorDto? Doctor { get; set; }
    public int? RoomId { get; set; }
    public RoomDto? Room { get; set; }
    public int? NurseId { get; set; }
    public NurseDto? Nurse { get; set; }
}
