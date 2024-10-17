using BeHealthy.Client.Persistance;
using BeHealthy.Shared.Interfaces;
using BeHealthy.Shared.Models.Dtos.Patient;
using Microsoft.AspNetCore.Components;

namespace BeHealthy.Client.Pages.Patients;

public partial class Index
{
    private string _createUserHref { get; set; } = default!;
    [Inject] IUserService _userService { get; set; } = default!;
    [Inject] IPatientService _patientService { get; set; } = default!;
    [Inject] NavigationManager _navigationManager { get; set; } = default!;

    private List<PatientDto> _patients { get; set; } = default!;

    private bool _isLoading = default;

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;
        _createUserHref = $"Account/Register?role=Patient&redirectUrl={RoutingEndpoints.PATIENTS_PAGE}";
        _patients = (await _patientService.GetAllPatientsAsync()).ToList();
        _isLoading = false;
    }

    //private async Task EditPatient(string id)
    //{
    //}

    private async Task DeletePatient(string id)
    {
        await _userService.DeleteUserAsync(id);
        _navigationManager.Refresh(forceReload: true);
    }
}
