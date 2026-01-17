using BeHealthy.Application.Dtos.Doctor;
using BeHealthy.Application.Extensions;
using BeHealthy.Application.Services;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Application.Validations.Doctor;
using BeHealthy.Common;
using BeHealthy.Components.Shared.Modals;
using BeHealthy.Domain;
using BeHealthy.Domain.Entities;
using BeHealthy.Models;
using BeHealthy.Services.Interfaces;
using BeHealthy.Shared.Locales;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace BeHealthy.Components.Pages.Doctors;

public partial class Doctors : BasePage
{
    [Inject] IDoctorService _doctorService { get; set; } = default!;
    [Inject] IPatientService _patientsService { get; set; } = default!;
    [Inject] NavigationManager _navigationManager { get; set; } = default!;
    [Inject] AuthenticationStateProvider _authenticationStateProvider { get; set; } = default!;

    private List<DoctorDto> _doctors { get; set; } = default!;

    private string _selectedView = "Card";
    private bool hasActionRights;
    private UserRole? _userRole;
    private string? _currentUserId;

    private PaginationState _paginationState = new();

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

        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        _userRole = authState.User.GetUserRoleEnum();
        _currentUserId = authState.User.GetUserId();

        await LoadDoctors(_currentUserId, _userRole);

        _paginationState.ItemsPerPage = _doctors.Count;
        hasActionRights = _userRole == UserRole.Admin;

        LoaderService.SetLoader(false);
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
        
        await InvokeAsync(StateHasChanged);
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

    private async Task BulkCreateDoctors((List<DoctorCreateDto> DoctorCreateDtos, bool UseValidation) result)
    {
        LoaderService.SetLoader(true);

        var useValidation = result.UseValidation;
        var validator = new DoctorCreateDtoValidator(false);

        foreach (var doctor in result.DoctorCreateDtos)
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
