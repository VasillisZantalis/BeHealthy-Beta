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
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace BeHealthy.Components.Pages.Doctors;

public partial class Doctors : BasePage
{
    [Inject] IDoctorService _doctorService { get; set; } = default!;
    [Inject] IPatientService _patientsService { get; set; } = default!;
    [Inject] ISpecialtyService _specialtyService { get; set; } = default!;
    [Inject] NavigationManager _navigationManager { get; set; } = default!;
    [Inject] AuthenticationStateProvider _authenticationStateProvider { get; set; } = default!;

    private List<DoctorDto> _doctors = new();
    private List<DoctorDto> filteredDoctors = new();

    // Filter state
    private string searchTerm = "";
    private string selectedSpecialtyId = "";

    // Specialty list for dropdown
    private List<SelectItem> specialties = new();

    private string _selectedView = "Grid";
    private bool hasActionRights;
    private UserRole? _userRole;
    private string? _currentUserId;

    private PaginationState _paginationState = new();

    protected override void OnInitialized()
    {
        SetBreadcrumbs();
    }

    protected override async Task OnInitializedAsync()
    {
        LoaderService.SetLoader(true);

        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        _userRole = authState.User.GetUserRoleEnum();
        _currentUserId = authState.User.GetUserId();

        await LoadDoctors(_currentUserId, _userRole);
        await LoadSpecialties();

        _paginationState.ItemsPerPage = _doctors.Count;
        hasActionRights = _userRole == UserRole.Admin;

        LoaderService.SetLoader(false);
    }

    private async Task LoadSpecialties()
    {
        var data = await _specialtyService.GetSpecialtiesAsync();

        specialties = data.Select(s => new SelectItem
        {
            Value = s.Id,
            Text = s.Name,
        }).ToList();
    }

    private void HandleSearch(string term)
    {
        searchTerm = term;
        ApplyFilters();
    }

    private void ApplyFilters()
    {
        filteredDoctors = _doctors;

        // Search filter
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            filteredDoctors = filteredDoctors.Where(d =>
                d.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                d.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                (d.Email != null && d.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            ).ToList();
        }

        // Specialty filter
        if (!string.IsNullOrEmpty(selectedSpecialtyId))
        {
            var specialtyId = int.Parse(selectedSpecialtyId);
            filteredDoctors = filteredDoctors.Where(d => d.Specialty?.Id == specialtyId).ToList();
        }

        StateHasChanged();
    }

    private void HandleClearFilters()
    {
        searchTerm = "";
        selectedSpecialtyId = "";
        filteredDoctors = _doctors;
        StateHasChanged();
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

        _doctors = userRole switch
        {
            UserRole.Patient when userId is not null => (await _patientsService.GetMyDoctorsAsync(userId)).ToList(),
            _ => (await _doctorService.GetAllDoctorsAsync()).ToList()
        };

        filteredDoctors = _doctors;
        
        StateHasChanged();
        LoaderService.SetLoader(false);
    }

    private void EditDoctor(int id)
    {
        _navigationManager.NavigateTo($"{RoutingEndpoints.DOCTORS_PAGE}/edit/{id}");
    }

    private void CreateDoctor()
    {
        _navigationManager.NavigateTo($"{RoutingEndpoints.DOCTORS_PAGE}/create");
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

            var response = await _doctorService.AddDoctorAsync(doctor);
            if (HandleServiceResponse(response)) continue;
        }
        await LoadDoctors(_currentUserId, _userRole);
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
        ModalService.Show<ConfirmDeleteModal>(
           new Dictionary<string, object?>
           {
               { nameof(ConfirmDeleteModal.OnConfirm), () => OnConfirmDeleteAsync(doctorId) }
           });
    }

    private async Task OnConfirmDeleteAsync(int doctorId)
    {
        await _doctorService.DeleteDoctorAsync(doctorId);
        await LoadDoctors(_currentUserId, _userRole);
    }
}
