using BeHealthy.Application.Dtos.Department;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Common;
using BeHealthy.Models;
using BeHealthy.Shared.Locales;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace BeHealthy.Components.Pages.Department;

public partial class Departments : BasePage
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
        _departments = (await _departmentService.GetAllDepartmentsAsync()).ToList();
        _paginationState.ItemsPerPage = _departments.Count;

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

    public void EditDepartment(int departmentId)
    {
        _navigationManager.NavigateTo($"{RoutingEndpoints.DEPARTMENTS_PAGE}/edit/{departmentId}");
    }

    private void ConfirmDelete(int departmentId)
    {
        ConfirmDeleteService.RequestDelete(async () =>
        {
            await _departmentService.DeleteDepartmentAsync(departmentId);
            _navigationManager.Refresh(forceReload: true);
        });
    }

    private void Create()
    {
        _navigationManager.NavigateTo($"{RoutingEndpoints.DEPARTMENTS_PAGE}/create");
    }
}
