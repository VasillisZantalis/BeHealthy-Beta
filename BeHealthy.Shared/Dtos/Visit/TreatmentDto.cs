namespace BeHealthy.Shared.Dtos.Visit;

public class TreatmentDto
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int VisitId { get; set; }
    public int? DiagnosisId { get; set; }
}
