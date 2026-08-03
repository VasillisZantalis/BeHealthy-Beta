namespace BeHealthy.Shared.Dtos.Prescription;

public class PrescriptionUpdateRequest
{
    public int Id { get; set; }
    public string Medication { get; set; } = string.Empty;
    public string Dosage { get; set; } = string.Empty;
}
