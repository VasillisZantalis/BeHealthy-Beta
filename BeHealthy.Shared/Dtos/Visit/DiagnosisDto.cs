namespace BeHealthy.Shared.Dtos.Visit;

public class DiagnosisDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public int VisitId { get; set; }
}
