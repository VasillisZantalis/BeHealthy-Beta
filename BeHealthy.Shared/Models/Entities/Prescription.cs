namespace BeHealthy.Shared.Models.Entities;

public class Prescription
{
    public int Id { get; set; }
    public string Medication { get; set; } = string.Empty;
    public string Dosage { get; set; } = string.Empty;
    public DateTime DatePrescribed { get; set; }

    // Patient relationship
    public string? PatientId { get; set; }
    public virtual ApplicationUser? Patient { get; set; }

    // Doctor relationship
    public string? DoctorId { get; set; }
    public virtual ApplicationUser? Doctor { get; set; }
}
