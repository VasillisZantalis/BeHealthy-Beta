namespace BeHealthy.Models;

public class Breadcrumb
{
    public required string Text { get; set; }
    public required string Link { get; set; }
    public bool Active { get; set; }
}
