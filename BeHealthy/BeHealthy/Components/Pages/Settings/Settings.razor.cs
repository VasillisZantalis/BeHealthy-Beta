using BeHealthy.Application.Services;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Common;
using BeHealthy.Domain;
using BeHealthy.Domain.Entities;
using BeHealthy.Models;
using BeHealthy.Shared.Locales;
using Microsoft.AspNetCore.Components;

namespace BeHealthy.Components.Pages.Settings;

public partial class Settings : BasePage
{
    private List<AppSetting> _settings = default!;

    private IEnumerable<IGrouping<SettingGroup, AppSetting>>? _settingsGroupedByArea;

    [Inject]
    IAppSettingsService AppSettingsService { get; set; } = default!;
    [Inject]
    private NavigationManager _navigationManager { get; set; } = default!;
    [Inject]
    private IDoctorService _doctorService { get; set; } = default!;

    private List<SelectItem> _doctorsSelect = new();


    protected override async Task OnInitializedAsync()
    {
        LoaderService.SetLoader(true);

        SetBreadcrumbs();
        _settings = (await AppSettingsService.GetAppSettingsAsync()).ToList();
        _settingsGroupedByArea = _settings
            .OrderBy(o => o.Group)
            .ThenBy(o => o.Key)
            .GroupBy(s => s.Group);

        if (_settings.Any(s => s.Key == "DefaultDepartmentSupervison"))
        {
            var doctors = (await _doctorService.GetAllDoctorsAsync()).ToList();

            _doctorsSelect = doctors.Select(s => new SelectItem
            {
                Value = s.Id,
                Text = s.FullName
            }).ToList();
            _doctorsSelect.Insert(0, new SelectItem { Value = 0, Text = Resource.PleaseSelect });
        }


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

    private async Task UpdateSettingValue(AppSetting setting)
    {
        LoaderService.SetLoader(true);
        await AppSettingsService.UpdateSettingAsync(setting);
        LoaderService.SetLoader(false);
        //await ToastrStateService.ShowSuccess(Resource.Success);
    }
}
