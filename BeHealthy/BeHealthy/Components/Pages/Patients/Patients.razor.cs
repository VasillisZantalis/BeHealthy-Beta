using BeHealthy.Application.Dtos.Patient;
using BeHealthy.Application.Extensions;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Application.Validations.Patient;
using BeHealthy.Common;
using BeHealthy.Components.Pages.Doctors;
using BeHealthy.Components.Shared.Modals;
using BeHealthy.Domain;
using BeHealthy.Domain.Entities;
using BeHealthy.Models;
using BeHealthy.Shared.Locales;
using BeHealthy.Shared.Parameters;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace BeHealthy.Components.Pages.Patients;

public partial class Patients : BasePage
{
    [Inject] IPatientService patientService { get; set; } = default!;
    [Inject] IDoctorService doctorService { get; set; } = default!;
    [Inject] NavigationManager navigationManager { get; set; } = default!;
    [Inject] AuthenticationStateProvider authenticationStateProvider { get; set; } = default!;

    private List<PatientDto> patients { get; set; } = new();

    private string selectedView = "Card";
    private bool hasActionRights;
    private bool hasEditRight;
    private bool hasDeleteRight;

    private string? firstNameFilter;

    private UserRole? userRole;
    private string? currentUserId;

    private PaginationState paginationState = new();

    private QuickGrid<PatientDto>? quickGrid;

    private bool showWizard = false;
    void ShowImportWizard() => showWizard = true;
    void HideImportWizard() => showWizard = false;

    protected override void OnInitialized()
    {
        SetBreadcrumbs();
    }

    protected override async Task OnInitializedAsync()
    {
        LoaderService.SetLoader(true);

        var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
        userRole = authState.User.GetUserRoleEnum();
        currentUserId = authState.User.GetUserId();

        await LoadPatients(currentUserId, userRole);

        paginationState.ItemsPerPage = patients.Count();

        hasEditRight = authState.User.GetUserRoleEnum() == UserRole.Admin;
        hasDeleteRight = authState.User.GetUserRoleEnum() == UserRole.Admin; 
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

    private async Task LoadPatients(string? doctorId, UserRole? userRole = UserRole.Admin)
    {
        LoaderService.SetLoader(true);

        patients = userRole switch
        {
            UserRole.Doctor when doctorId is not null => (await doctorService.GetMyPatientsAsync(doctorId)).ToList(),
            _ => (await patientService.GetAllPatientsAsync()).ToList()
        };
        await InvokeAsync(StateHasChanged);
        LoaderService.SetLoader(false);
    }

    private void EditPatient(int id)
    {
        navigationManager.NavigateTo($"{RoutingEndpoints.PATIENTS_PAGE}/edit/{id}");
    }

    private void CreatePatient()
    {
        navigationManager.NavigateTo($"{RoutingEndpoints.PATIENTS_PAGE}/create");
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

            var response = await patientService.AddPatientAsync(patient);
            if (HandleServiceResponse(response)) continue;
        }
        await LoadPatients(currentUserId, userRole);
        LoaderService.SetLoader(false);
    }

    private void ConfirmDelete(int patientId)
    {
        ModalService.Show<ConfirmDeleteModal>(
           new Dictionary<string, object?>
           {
               { nameof(ConfirmDeleteModal.OnConfirm), () => OnConfirmDeleteAsync(patientId) }
           });
    }

    private async Task OnConfirmDeleteAsync(int patientId)
    {
        await patientService.DeletePatientAsync(patientId);
        await LoadPatients(currentUserId, userRole);
    }
}
