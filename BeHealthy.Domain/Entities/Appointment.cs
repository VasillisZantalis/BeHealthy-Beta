using BeHealthy.Shared;

namespace BeHealthy.Domain.Entities;

public class Appointment
{
    public int Id { get; set; }
    public DateOnly AppointmentDate { get; set; }
    public TimeOnly AppointmentStartTime { get; set; }
    public TimeOnly AppointmentEndTime { get; set; }
    public string? Notes { get; set; }
    public AppointmentStatus Status { get; set; }
    public AppointmentReason Reason { get; set; }

    public int PatientId { get; set; }
    public Patient? Patient { get; set; }

    public int DoctorId { get; set; }
    public Doctor? Doctor { get; set; }

    public int? RoomId { get; set; }
    public Room? Room { get; set; }

    public int? NurseId { get; set; }
    public Nurse? Nurse { get; set; }
}
