using BeHealthy.Domain;

namespace BeHealthy.Models;

public class ToastrNotification
{
    public string? Message { get; set; }
    public Severity Severity { get; set; }
    public string? Icon { get; set; }
    public int? Duration { get; set; }
}
