using BeHealthy.Application.Dtos.Department;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Models;
using BeHealthy.Persistance;
using BeHealthy.Shared.Locales;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace BeHealthy.Components.Pages.Department;

public partial class Index : BasePage
{
    [Inject] IDepartmentService _departmentService { get; set; } = default!;
    [Inject] IPatientService _patientService { get; set; } = default!;
    [Inject] IDoctorService _doctorService { get; set; } = default!;
    [Inject] INurseService _nurseService { get; set; } = default!;
    [Inject] NavigationManager _navigationManager { get; set; } = default!;

    private List<DepartmentDto> _departments = new();

    private bool _hasActionRights;
    private PaginationState _paginationState = new();

    protected override async Task OnInitializedAsync()
    {
        LoaderService.SetLoader(true);

        SetBreadcrumbs();

        _hasActionRights = true;
        _paginationState.ItemsPerPage = 10;
        _departments = (await _departmentService.GetAllDepartmentsAsync()).ToList();

        LoaderService.SetLoader(false);
    }

    private void SetBreadcrumbs()
    {
        Breadcrumbs.SetBreadcrumbs(new List<Breadcrumb>()
        {
            new Breadcrumb(){ Text = Resource.Dashboard, Link = RoutingEndpoints.HOME_PAGE, Active = false },
            new Breadcrumb(){ Text = Resource.Departments, Link = string.Empty, Active = true },
        });
    }

    private void OnPageSizeChanged(ChangeEventArgs e)
    {
        if (e.Value is not null)
        {
            _paginationState.ItemsPerPage = int.Parse((string)e.Value);
        }
    }

    public void EditDepartment(int departmentId)
    {
        _navigationManager.NavigateTo($"/departments/edit/{departmentId}");
    }

    public async Task DeleteDepartment(int departmentId)
    {
        await _departmentService.DeleteDepartmentAsync(departmentId);
        _navigationManager.Refresh(forceReload: true);
    }

    private void Create()
    {
        _navigationManager.NavigateTo("/departments/create");
    }
}
