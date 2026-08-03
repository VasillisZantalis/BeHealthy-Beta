using BeHealthy.Frontend.Services.Interfaces;
using BeHealthy.Frontend.Common;
using BeHealthy.Shared;
using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Frontend.Models;
using BeHealthy.Shared.Locales;
using Microsoft.AspNetCore.Components;

namespace BeHealthy.Frontend.Components.Pages.Settings;

public partial class Settings : BasePage
{
    private List<AppSettingResponse> settings = default!;

    private IEnumerable<IGrouping<SettingGroup, AppSettingResponse>>? settingsGroupedByArea;

    [Inject]
    IAppSettingsService AppSettingsService { get; set; } = default!;
    [Inject]
    private IDoctorService DoctorService { get; set; } = default!;

    private List<SelectItem> doctorsSelect = new();

    protected override void OnInitialized()
    {
        SetBreadcrumbs();
    }

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;

        settings = (await AppSettingsService.GetAppSettingsAsync()).ToList();
        settingsGroupedByArea = settings
            .OrderBy(o => o.Group)
            .ThenBy(o => o.Key)
            .GroupBy(s => s.Group);

        if (settings.Any(s => s.Key == "DefaultDepartmentSupervison"))
        {
            var doctors = (await DoctorService.GetAllDoctorsSimpleAsync()).ToList();

            doctorsSelect = doctors.Select(s => new SelectItem
            {
                Value = s.Id,
                Text = s.FullName
            }).ToList();
            doctorsSelect.Insert(0, new SelectItem { Value = 0, Text = Resource.PleaseSelect });
        }


        IsLoading = false;
    }

    private void SetBreadcrumbs()
    {
        Breadcrumbs.SetBreadcrumbs(new List<Breadcrumb>()
        {
            new Breadcrumb(){ Text = Resource.Dashboard, Link = RoutingEndpoints.HOME_PAGE, Active = false },
            new Breadcrumb(){ Text = Resource.Settings, Link = string.Empty, Active = true },
        });
    }

    private async Task UpdateSettingValue(AppSettingResponse setting)
    {
        IsLoading = true;
        await AppSettingsService.UpdateSettingAsync(new AppSettingUpdateRequest { Key = setting.Key, Value = setting.Value });
        IsLoading = false;
        ToastService.ShowToast(Resource.Success, "success");
    }
}
