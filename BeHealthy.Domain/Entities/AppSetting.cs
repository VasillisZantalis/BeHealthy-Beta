using BeHealthy.Domain;

namespace BeHealthy.Domain.Entities;

public class AppSetting
{
    public int Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public SettingType Type { get; set; }
    public string Area { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime InsDate { get; set; }
}
