using BeHealthy.Application.Dtos.Doctor;
using BeHealthy.Application.Dtos.Patient;

namespace BeHealthy.Application.Dtos.Prescription;

public class PrescriptionDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public PatientSimpleDto Patient { get; set; } = new();
    public int DoctorId { get; set; }
    public DoctorSimpleDto Doctor { get; set; } = new();
    public string Medication { get; set; } = string.Empty;
    public string Dosage { get; set; } = string.Empty;
    public DateTime DatePrescribed { get; set; }
}
