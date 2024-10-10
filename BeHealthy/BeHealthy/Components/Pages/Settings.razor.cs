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
                Name = "Language",
                Type = SettingType.SingleSelect,
                StringValue = "English",
                InsDate = DateTime.UtcNow
            },
            new AppSetting
            {
                Id = 2,
                Name = "Color",
                Type = SettingType.MultiSelect,
                StringValue = "Red, Green",
                InsDate = DateTime.UtcNow
            },
            new AppSetting
            {
                Id = 3,
                Name = "Has Edit Privilege",
                Type = SettingType.Checkbox,
                BoolValue = true,
                InsDate = DateTime.UtcNow
            }
        };

    }

    //private bool GetCheckboxValue(AppSetting setting)
    //{
    //    return bool.TryParse(setting.Value, out var result) && result;
    //}

    //private void SetCheckboxValue(AppSetting setting, bool value)
    //{
    //    setting.Value = value.ToString();
    //}
}
