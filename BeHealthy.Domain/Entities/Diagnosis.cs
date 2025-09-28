namespace BeHealthy.Domain.Entities;

public class Diagnosis
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Notes { get; set; }

    public int VisitId { get; set; }
    public Visit? Visit { get; set; }

    public ICollection<Treatment> Treatments { get; set; } = new List<Treatment>();
}
