using BeHealthy.Application.Dtos.Patient;
using BeHealthy.Application.Extensions;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Components.Shared.Modals;
using BeHealthy.Domain;
using BeHealthy.Persistance;
using BeHealthy.States;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace BeHealthy.Components.Pages.Patients;

public partial class Index
{
    private string _createUserHref { get; set; } = default!;
    [Inject] IUserService _userService { get; set; } = default!;
    [Inject] IPatientService _patientService { get; set; } = default!;
    [Inject] NavigationManager _navigationManager { get; set; } = default!;
    [Inject] AuthenticationStateProvider _authenticationStateProvider { get; set; } = default!;
    [Inject] PrivilegeStateService _privilegeStateService { get; set; } = default!;

    private List<PatientDto> _patients { get; set; } = default!;
    private ConfirmDeleteModal _confirmDeleteModal = new();

    private bool _isLoading = default;
    private string _selectedView = "Card";
    private bool hasActionRights;
    private bool hasEditRight;
    private bool hasDeleteRight;

    private int deleteItemId;

    private PaginationState _paginationState = new();

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;

        _patients = (await _patientService.GetAllPatientsAsync()).ToList();
        _paginationState.ItemsPerPage = 10;

        hasEditRight = _privilegeStateService.HasPrivilege("CanEditAppointment");
        hasDeleteRight = _privilegeStateService.HasPrivilege("CanDeleteAppointment");
        hasActionRights = hasEditRight || hasDeleteRight;

        _isLoading = false;
    }

    private void OnPageSizeChanged(ChangeEventArgs e)
    {
        if (e.Value is not null)
        {
            _paginationState.ItemsPerPage = int.Parse((string)e.Value);
        }
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
