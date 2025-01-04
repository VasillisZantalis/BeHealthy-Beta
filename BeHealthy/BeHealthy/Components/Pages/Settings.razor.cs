using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Common;
using BeHealthy.Domain.Entities;
using BeHealthy.Models;
using BeHealthy.Shared.Locales;
using Microsoft.AspNetCore.Components;

namespace BeHealthy.Components.Pages;

public partial class Settings : BasePage
{
    private List<AppSetting> _settings = default!;

    private IEnumerable<IGrouping<string, AppSetting>>? _settingsGroupedByArea;

    [Inject] 
    IAppSettingsService AppSettingsService { get; set; } = default!;
    [Inject]
    private NavigationManager _navigationManager { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        LoaderService.SetLoader(true);

        SetBreadcrumbs(); 
        _settings = (await AppSettingsService.GetAppSettingsAsync()).ToList();
        _settingsGroupedByArea = _settings.GroupBy(s => s.Area);

        LoaderService.SetLoader(false);
    }

    private void SetBreadcrumbs()
    {
        Breadcrumbs.SetBreadcrumbs(new List<Breadcrumb>()
        {
            new Breadcrumb(){ Text = Resource.Dashboard, Link = RoutingEndpoints.HOME_PAGE, Active = false },
            new Breadcrumb(){ Text = Resource.Settings, Link = string.Empty, Active = true },
        });
    }

    private async Task UpdateSettingValue(AppSetting setting, string newValue)
    {
        setting.Value = newValue;

        await AppSettingsService.UpdateSettingAsync(setting);
        await ToastrStateService.ShowSuccess(Resource.Success, 500);
    }
}
