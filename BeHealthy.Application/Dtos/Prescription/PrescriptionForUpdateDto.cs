namespace BeHealthy.Application.Dtos.Prescription;

public class PrescriptionForUpdateDto
{
    public int Id { get; set; }
    public string Medication { get; set; } = string.Empty;
    public string Dosage { get; set; } = string.Empty;
}
