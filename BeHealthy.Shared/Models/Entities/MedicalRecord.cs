namespace BeHealthy.Shared.Models.Entities;

public class MedicalRecord
{
    public int Id { get; set; }
    public string Diagnosis { get; set; } = string.Empty;
    public string Treatment { get; set; } = string.Empty;
    public DateTime RecordDate { get; set; }

    public int PatientId { get; set; }
    //public required Patient Patient { get; set; }
    public ApplicationUser Patient { get; set; } = new();
}
