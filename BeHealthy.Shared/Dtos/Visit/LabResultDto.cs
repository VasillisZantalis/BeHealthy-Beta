namespace BeHealthy.Shared.Dtos.Visit;

public class LabResultDto
{
    public int Id { get; set; }
    public string TestName { get; set; } = string.Empty;
    public string ResultValue { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public string ReferenceRange { get; set; } = string.Empty;
    public DateTime ResultDate { get; set; }
    public int VisitId { get; set; }
}
