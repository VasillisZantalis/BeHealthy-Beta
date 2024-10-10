namespace BeHealthy.Shared.Models.Entities;

public class AppSetting
{
    public int Id { get; set; }
    public SettingType Type { get; set; }
    public string Area { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? StringValue { get; set; }
    public int? IntValue { get; set; }
    public bool BoolValue { get; set; }
    public DateTime InsDate { get; set; }
}
