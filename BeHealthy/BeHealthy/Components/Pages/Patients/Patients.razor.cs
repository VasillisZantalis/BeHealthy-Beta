using BeHealthy.Application.Dtos.Patient;
using BeHealthy.Application.Extensions;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Application.Validations.Patient;
using BeHealthy.Common;
using BeHealthy.Domain;
using BeHealthy.Models;
using BeHealthy.Shared.Locales;
using BeHealthy.Shared.Parameters;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace BeHealthy.Components.Pages.Patients;

public partial class Patients : BasePage
{
    [Inject] IPatientService _patientService { get; set; } = default!;
    [Inject] IDoctorService _doctorService { get; set; } = default!;
    [Inject] IPrivilegeService _privilegeService { get; set; } = default!;
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

    private bool showWizard = false;
    void ShowImportWizard() => showWizard = true;
    void HideImportWizard() => showWizard = false;

    protected override async Task OnInitializedAsync()
    {
        LoaderService.SetLoader(true);

        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var userRole = authState.User.GetUserRoleEnum();
        var doctorId = authState.User.GetUserId();

        await LoadPatients(_filters, doctorId, userRole);

        SetBreadcrumbs();

        _paginationState.ItemsPerPage = _patients.Count();

        hasEditRight = authState.User.GetUserRoleEnum() == UserRole.Admin || await _privilegeService.HasPrivilegeAsync(userRole!.Value, PrivilegeName.EditPatient);
        hasDeleteRight = authState.User.GetUserRoleEnum() == UserRole.Admin || await _privilegeService.HasPrivilegeAsync(userRole!.Value, PrivilegeName.DeletePatient);
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

        await LoadPatients(_filters, null, null);

        await _quickGrid!.RefreshDataAsync();
    }

    private async Task LoadPatients(PatientSearchingParameters filters, string? doctorId, UserRole? userRole = UserRole.Admin)
    {
        LoaderService.SetLoader(true);

        _patients = userRole switch
        {
            UserRole.Doctor when doctorId is not null => (await _doctorService.GetMyPatientsAsync(doctorId)).AsQueryable(),
            _ => (await _patientService.GetAllPatientsAsync(filters)).AsQueryable()
        };

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

    private async Task BulkCreatePatients((List<PatientCreateDto> patientForCreationDtos, bool UseValidation) result)
    {
        LoaderService.SetLoader(true);

        var useValidation = result.UseValidation;
        var validator = new PatientForCreationDtoValidator();

        foreach (var patient in result.patientForCreationDtos)
        {
            if (useValidation)
            {
                var validationResponse = await validator.ValidateAsync(patient);
                if (!validationResponse.IsValid)
                {
                    AlertModalStateService.Show(null, validationResponse.Errors.FirstOrDefault()?.ErrorMessage);
                    LoaderService.SetLoader(false);
                    return;
                }
            }

            var response = await _patientService.AddPatientAsync(patient);
            if (!response.Success)
            {
                AlertModalStateService.Show(Resource.Error, response.ErrorMessage!);
                LoaderService.SetLoader(false);
                return;
            }
        }
        LoaderService.SetLoader(false);
        _navigationManager.Refresh(true);
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
