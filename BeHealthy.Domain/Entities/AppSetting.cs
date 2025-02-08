using BeHealthy.Domain;

namespace BeHealthy.Domain.Entities;

public class AppSetting
{
    public int Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public SettingType Type { get; set; }
    public SettingGroup Group { get; set; }
    public string Value { get; set; } = string.Empty;
    public string Caption { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
