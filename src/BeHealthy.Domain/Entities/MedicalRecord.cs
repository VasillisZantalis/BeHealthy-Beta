namespace BeHealthy.Domain.Entities;

public class MedicalRecord
{
    public int Id { get; set; }
    public string? Notes { get; set; }
    public string? CreatedUserId { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime RecordDate { get; set; }

    public int PatientId { get; set; }
    public Patient? Patient { get; set; }

    public ICollection<Visit> Visits { get; set; } = new List<Visit>();
}
