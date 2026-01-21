using BeHealthy.Application.Dtos.Doctor;
using BeHealthy.Application.Extensions;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Application.Validations.Doctor;
using BeHealthy.Common;
using BeHealthy.Components.Shared.Modals;
using BeHealthy.Components.Shared.Wizards;
using BeHealthy.Domain;
using BeHealthy.Models;
using BeHealthy.Models.Enums;
using BeHealthy.Shared.Locales;
using BeHealthy.Shared.Parameters;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BeHealthy.Components.Pages.Doctors;

public partial class Doctors : BasePage
{
    [Inject] IDoctorService doctorService { get; set; } = default!;
    [Inject] IPatientService patientsService { get; set; } = default!;
    [Inject] ISpecialtyService specialtyService { get; set; } = default!;
    [Inject] NavigationManager navigationManager { get; set; } = default!;
    [Inject] AuthenticationStateProvider authenticationStateProvider { get; set; } = default!;

    private List<DoctorDto> doctors = new();
    private DoctorQueryParameters QueryParameters { get; set; } = new();
    private List<SelectItem> specialties = new();
    private HashSet<int> selectedDoctorIds = new();

    private string selectedView = "Grid";
    private bool hasActionRights;
    private UserRole? userRole;
    private string? currentUserId;

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

        await LoadDoctors(currentUserId, userRole);
        await LoadSpecialties();

        hasActionRights = userRole == UserRole.Admin;

        LoaderService.SetLoader(false);
    }

    private async Task LoadSpecialties()
    {
        var data = await specialtyService.GetSpecialtiesAsync();

        specialties = data.Select(s => new SelectItem
        {
            Value = s.Id,
            Text = s.Name,
        }).ToList();
    }

    private async Task HandleSearch(string term)
    {
        QueryParameters.SearchTerm = term;
        await LoadDoctors(currentUserId, userRole);
    }

    private async Task HandleSpecialtyFilter(int? specialtyId)
    {
        QueryParameters.SpecialtyId = specialtyId;
        await LoadDoctors(currentUserId, userRole);
    }

    private async Task HandleClearFilters()
    {
        if (string.IsNullOrEmpty(QueryParameters.SearchTerm) && QueryParameters.SpecialtyId is null)
            return;

        QueryParameters.SearchTerm = "";
        QueryParameters.SpecialtyId = null;
        await LoadDoctors(currentUserId, userRole);
    }

    private void SetBreadcrumbs()
    {
        Breadcrumbs.SetBreadcrumbs(new List<Breadcrumb>()
        {
            new Breadcrumb(){ Text = Resource.Dashboard, Link = RoutingEndpoints.HOME_PAGE, Active = false },
            new Breadcrumb(){ Text = Resource.Doctors, Link = string.Empty, Active = true },
        });
    }

    private async Task LoadDoctors(string? userId, UserRole? userRole = UserRole.Admin)
    {
        LoaderService.SetLoader(true);

        doctors = userRole switch
        {
            UserRole.Patient when userId is not null => (await patientsService.GetMyDoctorsAsync(userId)).ToList(),
            _ => (await doctorService.GetAllDoctorsAsync(QueryParameters)).ToList()
        };

        // Clear selection if doctors were reloaded
        selectedDoctorIds.Clear();

        await InvokeAsync(StateHasChanged);
        LoaderService.SetLoader(false);
    }

    private void ToggleDoctorSelection(int doctorId)
    {
        if (selectedDoctorIds.Contains(doctorId))
        {
            selectedDoctorIds.Remove(doctorId);
        }
        else
        {
            selectedDoctorIds.Add(doctorId);
        }
    }

    private void EditDoctor(int id)
    {
        navigationManager.NavigateTo($"{RoutingEndpoints.DOCTORS_PAGE}/edit/{id}");
    }

    private void CreateDoctor()
    {
        navigationManager.NavigateTo($"{RoutingEndpoints.DOCTORS_PAGE}/create");
    }

    private async Task BulkCreateDoctors((List<DoctorCreateDto> doctorCreateDtos, bool UseValidation) result)
    {
        LoaderService.SetLoader(true);

        var useValidation = result.UseValidation;
        var validator = new DoctorCreateDtoValidator(false);

        foreach (var doctor in result.doctorCreateDtos)
        {
            if (useValidation)
            {
                var validationResult = await validator.ValidateAsync(doctor);
                if (!validationResult.IsValid)
                {
                    AlertModalStateService.Show(null, validationResult.Errors.FirstOrDefault()?.ErrorMessage);
                    LoaderService.SetLoader(false);
                    return;
                }
            }

            var response = await doctorService.AddDoctorAsync(doctor);
            if (HandleServiceResponse(response)) continue;
        }
        await LoadDoctors(currentUserId, userRole);
        LoaderService.SetLoader(false);
    }

    void ShowImportWizard()
    {
        ModalService.Show<MassImportWizard<DoctorCreateDto>>(
            new Dictionary<string, object?>
            {
                { nameof(MassImportWizard<DoctorCreateDto>.Entity), ImportEntity.Doctor },
                { nameof(MassImportWizard<DoctorCreateDto>.OnSave), EventCallback.Factory.Create<(List<DoctorCreateDto> doctorCreateDtos, bool UseValidation)>(this, BulkCreateDoctors) },
            });
    }

    private void ConfirmDelete(int doctorId)
    {
        ConfirmDelete([doctorId]);
    }

    private void ConfirmBulkDelete()
    {
        ConfirmDelete(selectedDoctorIds.ToList());
    }

    private void ConfirmDelete(IEnumerable<int> doctorIds)
    {
        var doctorIdsList = doctorIds.ToList();

        ModalService.Show<ConfirmDeleteModal>(
           new Dictionary<string, object?>
           {
               { nameof(ConfirmDeleteModal.OnConfirm), () => OnConfirmDeleteAsync(doctorIdsList) },
           });
    }

    private async Task OnConfirmDeleteAsync(IEnumerable<int> doctorIds)
    {
        LoaderService.SetLoader(true);

        foreach (var doctorId in doctorIds)
        {
            await doctorService.DeleteDoctorAsync(doctorId);
        }

        selectedDoctorIds.Clear();
        await LoadDoctors(currentUserId, userRole);

        LoaderService.SetLoader(false);
    }
}
