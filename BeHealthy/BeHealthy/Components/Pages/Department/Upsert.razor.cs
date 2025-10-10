using BeHealthy.Application.Dtos.Department;
using BeHealthy.Application.Dtos.Doctor;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Common;
using BeHealthy.Domain;
using BeHealthy.Models;
using BeHealthy.Shared.Locales;
using Microsoft.AspNetCore.Components;

namespace BeHealthy.Components.Pages.Department;

public partial class Upsert : BasePage
{
    [Parameter]
    public int? id { get; set; }
    private bool IsEditMode => id.HasValue;
    private DepartmentTabs activeTab = DepartmentTabs.GeneralData;
    private DepartmentDto DepartmentDto = new();
    private List<DoctorDto> Doctors = new();

    [Inject]
    private IDepartmentService _departmentService { get; set; } = default!;

    [Inject]
    private IDoctorService _doctorService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        LoaderService.SetLoader(true);

        SetBreadcrumbs();
        Doctors = (await _doctorService.GetAllDoctorsAsync()).ToList();

        if (IsEditMode && id.HasValue)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id.Value);
            DepartmentDto = department;
        }

        LoaderService.SetLoader(false);
    }

    private void SetBreadcrumbs()
    {
        Breadcrumbs.SetBreadcrumbs(new List<Breadcrumb>()
        {
            new Breadcrumb(){ Text = Resource.Dashboard, Link = RoutingEndpoints.HOME_PAGE, Active = false },
            new Breadcrumb(){ Text = Resource.Departments, Link = RoutingEndpoints.DEPARTMENTS_PAGE, Active = false },
            new Breadcrumb(){ Text = !IsEditMode ? Resource.Create : Resource.Edit, Link = string.Empty, Active = true },
        });

        if (IsEditMode && id.HasValue)
        {
            Breadcrumbs.AddBreadcrumb(new Breadcrumb() { Text = id.Value.ToString(), Link = string.Empty, Active = true });
        }
            
    }
}
