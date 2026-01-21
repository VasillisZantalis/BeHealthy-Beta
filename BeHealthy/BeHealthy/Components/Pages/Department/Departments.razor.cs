using BeHealthy.Application.Dtos.Department;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Common;
using BeHealthy.Components.Shared.Modals;
using BeHealthy.Domain;
using BeHealthy.Models;
using BeHealthy.Services;
using BeHealthy.Shared.Locales;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace BeHealthy.Components.Pages.Department;

public partial class Departments : BasePage
{
    [Inject] IDepartmentService departmentService { get; set; } = default!;
    [Inject] IPatientService PatientService { get; set; } = default!;
    [Inject] IDoctorService DoctorService { get; set; } = default!;
    [Inject] INurseService NurseService { get; set; } = default!;
    [Inject] NavigationManager NavigationManager { get; set; } = default!;

    private List<DepartmentDto> departments = new();

    private bool hasActionRights;
    private PaginationState paginationState = new();

    protected override void OnInitialized()
    {
        SetBreadcrumbs();
    }

    protected override async Task OnInitializedAsync()
    {
        LoaderService.SetLoader(true);

        await LoadDepartments();
        hasActionRights = true;
        paginationState.ItemsPerPage = departments.Count;

        LoaderService.SetLoader(false);
    }

    private async Task LoadDepartments()
    {
        departments = (await departmentService.GetAllDepartmentsAsync()).ToList();
        await InvokeAsync(StateHasChanged);
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
        NavigationManager.NavigateTo($"{RoutingEndpoints.DEPARTMENTS_PAGE}/edit/{departmentId}");
    }

    private void ConfirmDelete(int departmentId)
    {
        ModalService.Show<ConfirmDeleteModal>(
           new Dictionary<string, object?>
           {
               { nameof(ConfirmDeleteModal.OnConfirm), () => OnConfirmDeleteAsync(departmentId) }
           });
    }

    private async Task OnConfirmDeleteAsync(int departmentId)
    {
        await departmentService.DeleteDepartmentAsync(departmentId);
        await LoadDepartments();
    }

    private void Create()
    {
        NavigationManager.NavigateTo($"{RoutingEndpoints.DEPARTMENTS_PAGE}/create");
    }
}
