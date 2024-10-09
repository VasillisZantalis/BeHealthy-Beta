using BeHealthy.Shared.Models;
using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Components.Pages;

public partial class Settings
{
    private List<AppSetting> _settings = default!;

    protected override void OnInitialized()
    {
        _settings = new List<AppSetting>
        {
            new AppSetting
            {
                Id = 1,
                Key = "Language",
                Type = SettingType.SingleSelect, // Or use an enum if you have one
                Value = "English", // Default value
                InsDate = DateTime.UtcNow
            },
            new AppSetting
            {
                Id = 2,
                Key = "Color",
                Type = SettingType.MultiSelect,
                Value = "Red, Green", // Default selected values
                InsDate = DateTime.UtcNow
            },
            new AppSetting
            {
                Id = 3,
                Key = "Has Edit Privilege",
                Type = SettingType.Checkbox,
                Value = "true",
                InsDate = DateTime.UtcNow
            }
        };

    }

    private bool GetCheckboxValue(AppSetting setting)
    {
        return bool.TryParse(setting.Value, out var result) && result;
    }

    private void SetCheckboxValue(AppSetting setting, bool value)
    {
        setting.Value = value.ToString();
    }
}
