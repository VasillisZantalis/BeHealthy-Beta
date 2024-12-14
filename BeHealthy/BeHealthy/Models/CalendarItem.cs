using System.Text.Json.Serialization;

namespace BeHealthy.Models;

public class CalendarItem
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;
    [JsonPropertyName("start")]
    public string Start { get; set; } = string.Empty;
    [JsonPropertyName("end")]
    public string? End { get; set; } = null;
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
    [JsonPropertyName("backgroundColor")]
    public string? BackgroundColor { get; set; } = null;
    [JsonPropertyName("borderColor")]
    public string? BorderColor { get; set; } = null;
    [JsonPropertyName("color")]
    public string? Color { get; set; }
}
