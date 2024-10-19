using BeHealthy.Client.Persistance;
using BeHealthy.Shared.Interfaces;
using BeHealthy.Shared.Models.Dtos.Patient;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace BeHealthy.Client.Pages.Patients;

public partial class Index
{
    private string _createUserHref { get; set; } = default!;
    [Inject] IUserService _userService { get; set; } = default!;
    [Inject] IPatientService _patientService { get; set; } = default!;
    [Inject] NavigationManager _navigationManager { get; set; } = default!;

    private List<PatientDto> _patients { get; set; } = default!;

    private bool _isLoading = default;
    private string _selectedView = "Card";

    private PaginationState _paginationState = new();

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;
        _createUserHref = $"Account/Register?role=Patient&redirectUrl={RoutingEndpoints.PATIENTS_PAGE}";
        _patients = (await _patientService.GetAllPatientsAsync()).ToList();
        _paginationState.ItemsPerPage = 10;
        _isLoading = false;
    }

    private async Task EditPatient(int id)
    {
    }

    private void OnPageSizeChanged(ChangeEventArgs e)
    {
        if (e.Value is not null)
        {
            _paginationState.ItemsPerPage = int.Parse((string)e.Value);
        }
    }

    private async Task DeletePatient(int id)
    {
        await _patientService.DeletePatientAsync(id);
        _navigationManager.Refresh(forceReload: true);
    }
}
