using BeHealthy.Application.Dtos.Doctor;
using BeHealthy.Application.Dtos.Patient;
using BeHealthy.Application.Extensions;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Application.Validations.Patient;
using BeHealthy.Common;
using BeHealthy.Components.Pages.Doctors;
using BeHealthy.Components.Shared.Modals;
using BeHealthy.Components.Shared.Wizards;
using BeHealthy.Domain;
using BeHealthy.Domain.Entities;
using BeHealthy.Models;
using BeHealthy.Models.Enums;
using BeHealthy.Shared.Locales;
using BeHealthy.Shared.Parameters;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace BeHealthy.Components.Pages.Patients;

public partial class Patients : BasePage
{
    [Inject] IPatientService PatientService { get; set; } = default!;
    [Inject] IDoctorService DoctorService { get; set; } = default!;
    [Inject] NavigationManager NavigationManager { get; set; } = default!;
    [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

    private List<PatientDto> patients { get; set; } = new();
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

        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        userRole = authState.User.GetUserRoleEnum();
        currentUserId = authState.User.GetUserId();

        await LoadPatients(currentUserId, userRole);

        hasEditRight = authState.User.GetUserRoleEnum() == UserRole.Admin;
        hasDeleteRight = authState.User.GetUserRoleEnum() == UserRole.Admin; 
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
        ModalService.Show<MassImportWizard<DoctorCreateDto>>(
            new Dictionary<string, object?>
            {
                { nameof(MassImportWizard<DoctorCreateDto>.Entity), ImportEntity.Patient },
                { nameof(MassImportWizard<DoctorCreateDto>.OnSave), EventCallback.Factory.Create<(List<PatientCreateDto> patientCreateDtos, bool UseValidation)>(this, BulkCreatePatients) },
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

    private async Task BulkCreatePatients((List<PatientCreateDto> patientForCreationDtos, bool UseValidation) result)
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
