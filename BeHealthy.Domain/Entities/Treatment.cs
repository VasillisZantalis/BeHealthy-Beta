namespace BeHealthy.Domain.Entities;

public class Treatment
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public int VisitId { get; set; }
    public Visit Visit { get; set; } = new();

    public int? DiagnosisId { get; set; }
    public Diagnosis Diagnosis { get; set; } = new();

    public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}
