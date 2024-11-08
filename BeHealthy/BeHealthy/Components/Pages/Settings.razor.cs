using BeHealthy.Services.Interfaces;
using BeHealthy.Shared.Models.Entities;
using Microsoft.AspNetCore.Components;

namespace BeHealthy.Components.Pages;

public partial class Settings
{
    private List<AppSetting> _settings = default!;

    private IEnumerable<IGrouping<string, AppSetting>>? _settingsGroupedByArea;

    [Inject] IAppSettingsService AppSettingsService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _settings = (await AppSettingsService.GetAppSettingsAsync()).ToList();
        _settingsGroupedByArea = _settings.GroupBy(s => s.Area);
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
