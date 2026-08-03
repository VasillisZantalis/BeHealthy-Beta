using BeHealthy.Shared.Dtos.Doctor;
using BeHealthy.Frontend.Services.CurrentUser;
using BeHealthy.Shared.Dtos.Patient;
using BeHealthy.Frontend.Extensions;
using BeHealthy.Frontend.Services.Interfaces;
using BeHealthy.Frontend.Validations.Patient;
using BeHealthy.Frontend.Common;
using BeHealthy.Frontend.Components.Pages.Doctors;
using BeHealthy.Frontend.Components.Shared.Modals;
using BeHealthy.Frontend.Components.Shared.Wizards;
using BeHealthy.Shared;
using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Frontend.Models;
using BeHealthy.Frontend.Models.Enums;
using BeHealthy.Shared.Locales;
using BeHealthy.Shared.Parameters;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace BeHealthy.Frontend.Components.Pages.Patients;

public partial class Patients : BasePage
{
    [Inject] IPatientService PatientService { get; set; } = default!;
    [Inject] IDoctorService DoctorService { get; set; } = default!;
    [Inject] NavigationManager NavigationManager { get; set; } = default!;
    [Inject] ICurrentUserService CurrentUser { get; set; } = default!;

    private List<PatientResponse> patients { get; set; } = new();
    private PatientQueryParameters QueryParameters { get; set; } = new();
    private HashSet<int> selectedPatientIds = new();

    private string selectedView = "Grid";
    private bool hasActionRights;
    private bool hasEditRight;
    private bool hasDeleteRight;

    private UserRole? userRole;
    private string? currentUserId;

    protected override void OnInitialized()
    {
        SetBreadcrumbs();
    }

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;

        userRole = CurrentUser.Role;
        currentUserId = CurrentUser.UserId;

        await LoadPatients(currentUserId, userRole);

        hasEditRight = CurrentUser.Role == UserRole.Admin;
        hasDeleteRight = CurrentUser.Role == UserRole.Admin; 
        hasActionRights = hasEditRight || hasDeleteRight;
        IsLoading = false;
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
        IsLoading = true;

        patients = userRole switch
        {
            UserRole.Doctor when doctorId is not null => (await DoctorService.GetMyPatientsAsync(doctorId)).ToList(),
            _ => (await PatientService.GetAllPatientsAsync(QueryParameters)).ToList()
        };

        selectedPatientIds.Clear();

        StateHasChanged();
        IsLoading = false;
    }

    private void TogglePatientSelection(int patientId)
    {
        if (selectedPatientIds.Contains(patientId))
        {
            selectedPatientIds.Remove(patientId);
        }
        else
        {
            selectedPatientIds.Add(patientId);
        }
    }
    private async Task HandleSearch(string term)
    {
        QueryParameters.SearchTerm = term;
        await LoadPatients(currentUserId, userRole);
    }

    private async Task HandleClearFilters()
    {
        if (string.IsNullOrEmpty(QueryParameters.SearchTerm)) return;

        QueryParameters.SearchTerm = "";
        await LoadPatients(currentUserId, userRole);
    }

    void ShowImportWizard()
    {
        ModalService.Show<MassImportWizard<DoctorCreateRequest>>(
            new Dictionary<string, object?>
            {
                { nameof(MassImportWizard<DoctorCreateRequest>.Entity), ImportEntity.Patient },
                { nameof(MassImportWizard<DoctorCreateRequest>.OnSave), EventCallback.Factory.Create<(List<PatientCreateRequest> patientCreateDtos, bool UseValidation)>(this, BulkCreatePatients) },
            });
    }

    private void EditPatient(int id)
    {
        NavigationManager.NavigateTo($"{RoutingEndpoints.PATIENTS_PAGE}/edit/{id}");
    }

    private void CreatePatient()
    {
        NavigationManager.NavigateTo($"{RoutingEndpoints.PATIENTS_PAGE}/create");
    }

    private async Task BulkCreatePatients((List<PatientCreateRequest> patientForCreationDtos, bool UseValidation) result)
    {
        IsLoading = true;

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
                    IsLoading = false;
                    return;
                }
            }

            var response = await PatientService.AddPatientAsync(patient);
            if (HandleServiceResponse(response)) continue;
        }
        await LoadPatients(currentUserId, userRole);
        IsLoading = false;
    }

    private void ConfirmDelete(int patientId)
    {
        ConfirmDelete([patientId]);
    }

    private void ConfirmBulkDelete()
    {
        ConfirmDelete(selectedPatientIds.ToList());
    }

    private void ConfirmDelete(IEnumerable<int> patientIds)
    {
        var patientIdsList = patientIds.ToList();

        ModalService.Show<ConfirmDeleteModal>(
           new Dictionary<string, object?>
           {
               { nameof(ConfirmDeleteModal.OnConfirm), () => OnConfirmDeleteAsync(patientIdsList) },
           });
    }

    private async Task OnConfirmDeleteAsync(IEnumerable<int> patientIds)
    {
        IsLoading = true;

        foreach (var patientId in patientIds)
        {
            await PatientService.DeletePatientAsync(patientId);
        }
        
        IsLoading = false;

        await LoadPatients(currentUserId, userRole);
    }
}
