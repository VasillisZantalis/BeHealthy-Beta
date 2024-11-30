using BeHealthy.Application.Dtos.Doctor;
using BeHealthy.Application.Dtos.Nurse;
using BeHealthy.Application.Dtos.Patient;
using BeHealthy.Application.Dtos.Room;
using BeHealthy.Domain;
using BeHealthy.Shared.Locales;
using System.ComponentModel.DataAnnotations;

namespace BeHealthy.Application.Dtos.Appointment;

public class AppointmentDto
{
    public int Id { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string Notes { get; set; } = string.Empty;
    public AppointmentStatus Status { get; set; }
    public AppointmentReason Reason { get; set; }
    public int Duration { get; set; }
    public int PatientId { get; set; }
    public PatientDto? Patient { get; set; }
    public int DoctorId { get; set; }
    public DoctorDto? Doctor { get; set; }
    public int? RoomId { get; set; }
    public RoomDto? Room { get; set; }
    public int? NurseId { get; set; }
    public NurseDto? Nurse { get; set; }
}
