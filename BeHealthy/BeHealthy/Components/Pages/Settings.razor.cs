using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Domain.Entities;
using BeHealthy.Models;
using BeHealthy.Persistance;
using BeHealthy.Shared.Locales;
using Microsoft.AspNetCore.Components;

namespace BeHealthy.Components.Pages;

public partial class Settings : BasePage
{
    private List<AppSetting> _settings = default!;

    private IEnumerable<IGrouping<string, AppSetting>>? _settingsGroupedByArea;

    [Inject] IAppSettingsService AppSettingsService { get; set; } = default!;

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


    //private bool GetCheckboxValue(AppSetting setting)
    //{
    //    return bool.TryParse(setting.Value, out var result) && result;
    //}

    //private void SetCheckboxValue(AppSetting setting, bool value)
    //{
    //    setting.Value = value.ToString();
    //}
}
