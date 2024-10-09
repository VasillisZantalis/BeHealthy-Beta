namespace BeHealthy.Shared.Models.Entities;

public class AppSetting
{
    public int Id { get; set; }
    public SettingType Type { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public DateTime InsDate { get; set; }
    public string EnumType { get; set; } = string.Empty;
}
