namespace BeHealthy.Shared.Models.Entities;

public class Prescription
{
    public int Id { get; set; }
    public string Medication { get; set; } = string.Empty;
    public string Dosage { get; set; } = string.Empty;
    public DateTime DatePrescribed { get; set; }

    public string? PatientId { get; set; }
    public virtual ApplicationUser? Patient { get; set; }

    public string? DoctorId { get; set; }
    public virtual ApplicationUser? Doctor { get; set; }
}
