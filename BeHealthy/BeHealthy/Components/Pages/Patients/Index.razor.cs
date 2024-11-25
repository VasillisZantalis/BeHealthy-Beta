using BeHealthy.Application.Dtos.Patient;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Components.Shared.Controls;
using BeHealthy.Components.Shared.Modals;
using BeHealthy.Models;
using BeHealthy.Persistance;
using BeHealthy.Shared.Locales;
using BeHealthy.States;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace BeHealthy.Components.Pages.Patients;

public partial class Index : BasePage
{
    [Inject] IPatientService _patientService { get; set; } = default!;
    [Inject] NavigationManager _navigationManager { get; set; } = default!;
    [Inject] AuthenticationStateProvider _authenticationStateProvider { get; set; } = default!;

    private List<PatientDto> _patients { get; set; } = default!;
    private ConfirmDeleteModal _confirmDeleteModal = new();

    private string _selectedView = "Card";
    private bool hasActionRights;
    private bool hasEditRight;
    private bool hasDeleteRight;
    private int deleteItemId;

    private PaginationState _paginationState = new();
    private FilterParams _filters = new();

    protected override async Task OnInitializedAsync()
    {
        LoaderService.SetLoader(true);
        await LoadPatients(_filters);

        SetBreadcrumbs();

        _paginationState.ItemsPerPage = 10;

        hasEditRight = await PrivilegeStateService.HasPrivilegeAsync("CanEditAppointment");
        hasDeleteRight = await PrivilegeStateService.HasPrivilegeAsync("CanDeleteAppointment");
        hasActionRights = hasEditRight || hasDeleteRight;
        LoaderService.SetLoader(false);
    }

    private void SetBreadcrumbs() 
    {
        Breadcrumbs.ResetBreadcrumb();
        Breadcrumbs.AddBreadcrumb(new Breadcrumb 
        { 
            Text = Resource.Dashboard,
            Link = RoutingEndpoints.HOME_PAGE,
            Active = true 
        });
    }

    private void OnPageSizeChanged(ChangeEventArgs e)
    {
        if (e.Value is not null)
        {
            _paginationState.ItemsPerPage = int.Parse((string)e.Value);
        }
    }

    private async Task HandleFilterApplied(FilterParams filters)
    {
        _filters = filters;

        await LoadPatients(_filters);
    }

    public async Task LoadPatients(FilterParams filters)
    {
        LoaderService.SetLoader(true);
        _patients = (await _patientService.GetAllPatientsAsync(filters.FirstName, filters.LastName)).ToList();
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

    private void ConfirmDelete(int id)
    {
        deleteItemId = id;
        _confirmDeleteModal.HandleOpen();
    }

    private async Task OnDeleteConfirmed(bool confirmed)
    {
        if (confirmed)
        {
            await _patientService.DeletePatientAsync(deleteItemId);
            _navigationManager.Refresh(forceReload: true);
        }
    }
}
