namespace BeHealthy.Models;

public class CalendarItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Start { get; set; } = string.Empty;
    public string? End { get; set; } = null;
    public string Description { get; set; } = string.Empty;
    public string? BackgroundColor { get; set; } = null;
    public string? BorderColor { get; set; } = null;
    public string? Color { get; set; }
}
