using BeHealthy.Application.Dtos.Patient;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Common;
using BeHealthy.Domain;
using BeHealthy.Models;
using BeHealthy.Shared.Locales;
using BeHealthy.Shared.Parameters;
using BeHealthy.States;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace BeHealthy.Components.Pages.Patients;

public partial class Patients : BasePage
{
    [Inject] IPatientService _patientService { get; set; } = default!;
    [Inject] NavigationManager _navigationManager { get; set; } = default!;
    [Inject] AuthenticationStateProvider _authenticationStateProvider { get; set; } = default!;

    private IQueryable<PatientDto> _patients { get; set; } = default!;
    IQueryable<PatientDto> _filteredPatients
    {
        get
        {
            var result = _patients;

            if (!string.IsNullOrEmpty(firstNameFilter))
            {
                result = result.Where(w => w.FirstName.Contains(firstNameFilter));
            }

            return result;
        }
    }

    private string _selectedView = "Card";
    private bool hasActionRights;
    private bool hasEditRight;
    private bool hasDeleteRight;

    private string? firstNameFilter;

    private PaginationState _paginationState = new();
    private PatientSearchingParameters _filters = new();

    private QuickGrid<PatientDto>? _quickGrid;

    protected override async Task OnInitializedAsync()
    {
        LoaderService.SetLoader(true);
        await LoadPatients(_filters);

        SetBreadcrumbs();

        _paginationState.ItemsPerPage = 10;

        hasEditRight = await PrivilegeStateService.HasPrivilegeAsync(PrivilegeName.EditAppointments);
        hasDeleteRight = await PrivilegeStateService.HasPrivilegeAsync(PrivilegeName.DeleteAppointments);
        hasActionRights = hasEditRight || hasDeleteRight;
        LoaderService.SetLoader(false);
    }

    private void SetBreadcrumbs()
    {
        Breadcrumbs.SetBreadcrumbs(new List<Breadcrumb>()
        {
            new Breadcrumb(){ Text = Resource.Dashboard, Link = RoutingEndpoints.HOME_PAGE, Active = false },
            new Breadcrumb(){ Text = Resource.Patients, Link = string.Empty, Active = true },
        });
    }

    private async Task HandleFilterApplied(PatientSearchingParameters filters)
    {
        _filters = filters;

        await LoadPatients(_filters);

        await _quickGrid!.RefreshDataAsync();
    }

    public async Task LoadPatients(PatientSearchingParameters filters)
    {
        LoaderService.SetLoader(true);
        _patients = (await _patientService.GetAllPatientsAsync(filters)).AsQueryable();
        LoaderService.SetLoader(false);
    }

    private void EditPatient(int id)
    {
        _navigationManager.NavigateTo($"{RoutingEndpoints.PATIENTS_PAGE}/edit/{id}");
    }

    private void CreatePatient()
    {
        _navigationManager.NavigateTo($"{RoutingEndpoints.PATIENTS_PAGE}/create");
    }

    private void ConfirmDelete(int patientId)
    {
        ConfirmDeleteService.RequestDelete(async () =>
        {
            await _patientService.DeletePatientAsync(patientId);
            _navigationManager.Refresh(forceReload: true);
        });
    }
}
