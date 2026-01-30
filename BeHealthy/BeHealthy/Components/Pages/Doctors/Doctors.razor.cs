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
    [Inject] IDoctorService DoctorService { get; set; } = default!;
    [Inject] IPatientService PatientsService { get; set; } = default!;
    [Inject] ISpecialtyService SpecialtyService { get; set; } = default!;
    [Inject] NavigationManager NavigationManager { get; set; } = default!;
    [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

    private DoctorQueryParameters QueryParameters { get; set; } = new();
    private List<DoctorDto> doctors = new();
    private List<SelectItem> specialties = new();
    private HashSet<int> selectedDoctorIds = new();

    private string selectedView = "Grid";
    private bool hasActionRights;
    private UserRole? userRole;
    private string? currentUserId;
    private bool isLoading = false;

    protected override void OnInitialized()
    {
        SetBreadcrumbs();
    }

    protected override async Task OnInitializedAsync()
    {
        isLoading = true;
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        userRole = authState.User.GetUserRoleEnum();
        currentUserId = authState.User.GetUserId();

        await LoadDoctors();
        await LoadSpecialties();

        hasActionRights = userRole == UserRole.Admin;
        isLoading = false;
    }

    private async Task LoadSpecialties()
    {
        var data = await SpecialtyService.GetSpecialtiesAsync();

        specialties = data.Select(s => new SelectItem
        {
            Value = s.Id,
            Text = s.Name,
        }).ToList();
        specialties.Insert(0, new SelectItem { Value = 0, Text = Resource.All });
    }

    private async Task HandleSearch(string term)
    {
        QueryParameters.SearchTerm = term;
        await LoadDoctors();
    }

    private async Task HandleSpecialtyFilter(int? specialtyId)
    {
        QueryParameters.SpecialtyId = specialtyId;
        await LoadDoctors();
    }

    private async Task HandlePageChanged(int page)
    {
        QueryParameters.PageNumber = page;
        await LoadDoctors();
    }

    private async Task HandlePageSizeChanged(int pageSize)
    {
        QueryParameters.PageSize = pageSize;
        QueryParameters.PageNumber = 1;
        await LoadDoctors();
    }

    private async Task HandleClearFilters()
    {
        if (string.IsNullOrEmpty(QueryParameters.SearchTerm) && QueryParameters.SpecialtyId is null)
            return;

        QueryParameters.SearchTerm = "";
        QueryParameters.SpecialtyId = null;
        await LoadDoctors();
    }

    private void SetBreadcrumbs()
    {
        Breadcrumbs.SetBreadcrumbs(new List<Breadcrumb>()
        {
            new Breadcrumb(){ Text = Resource.Dashboard, Link = RoutingEndpoints.HOME_PAGE, Active = false },
            new Breadcrumb(){ Text = Resource.Doctors, Link = string.Empty, Active = true },
        });
    }

    private async Task LoadDoctors()
    {
        var role = userRole ?? UserRole.Admin;
        var userId = currentUserId;

        isLoading = true;
        doctors = (role switch
        {
            UserRole.Patient when userId is not null => await PatientsService.GetMyDoctorsAsync(userId),
            _ => await DoctorService.GetAllDoctorsAsync(QueryParameters)
        }).ToList();

        selectedDoctorIds.Clear();
        isLoading = false;
        StateHasChanged();
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
        NavigationManager.NavigateTo($"{RoutingEndpoints.DOCTORS_PAGE}/edit/{id}");
    }

    private void CreateDoctor()
    {
        NavigationManager.NavigateTo($"{RoutingEndpoints.DOCTORS_PAGE}/create");
    }

    private async Task BulkCreateDoctors((List<DoctorCreateDto> doctorCreateDtos, bool UseValidation) result)
    {
        isLoading = true;
        var validator = result.UseValidation ? new DoctorCreateDtoValidator(false) : null;

        foreach (var doctor in result.doctorCreateDtos)
        {
            if (validator != null)
            {
                var validationResult = await validator.ValidateAsync(doctor);
                if (!validationResult.IsValid)
                {
                    AlertModalStateService.Show(null, validationResult.Errors.FirstOrDefault()?.ErrorMessage);
                    isLoading = false;
                    return;
                }
            }

            var response = await DoctorService.AddDoctorAsync(doctor);
            if (!HandleServiceResponse(response))
            {
                isLoading = false;
                return;
            }
        }
        
        await LoadDoctors();
        isLoading = false;
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
        ModalService.Show<ConfirmDeleteModal>(
           new Dictionary<string, object?>
           {
               { nameof(ConfirmDeleteModal.OnConfirm), () => OnConfirmDeleteAsync(doctorIds) },
           });
    }

    private async Task OnConfirmDeleteAsync(IEnumerable<int> doctorIds)
    {
        isLoading = true;
        foreach (var doctorId in doctorIds)
        {
            await DoctorService.DeleteDoctorAsync(doctorId);
        }

        await LoadDoctors();
        isLoading = false;
    }
}
