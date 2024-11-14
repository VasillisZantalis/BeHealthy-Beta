using BeHealthy.Components.Shared.Modals;
using BeHealthy.Extensions;
using BeHealthy.Persistance;
using BeHealthy.Services.Interfaces;
using BeHealthy.Shared.Models;
using BeHealthy.Shared.Models.Dtos.Patient;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace BeHealthy.Components.Pages.Patients;

public partial class Index
{
    private string _createUserHref { get; set; } = default!;
    [Inject] IUserService _userService { get; set; } = default!;
    [Inject] IPatientService _patientService { get; set; } = default!;
    [Inject] IPrivilegeService _privilegeService { get; set; } = default!;
    [Inject] NavigationManager _navigationManager { get; set; } = default!;
    [Inject] AuthenticationStateProvider _authenticationStateProvider { get; set; } = default!;

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
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var userRole = Enum.Parse<UserRole>(authState.User.GetUserRole());

        _patients = (await _patientService.GetAllPatientsAsync()).ToList();
        _paginationState.ItemsPerPage = 10;
        hasEditRight = await _privilegeService.HasPrivilege(userRole, "CanEditAppointment");
        hasDeleteRight = await _privilegeService.HasPrivilege(userRole, "CanDeleteAppointment");
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
