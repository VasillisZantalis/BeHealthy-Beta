using BeHealthy.Application.Dtos.Doctor;
using BeHealthy.Application.Extensions;
using BeHealthy.Application.Services;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Common;
using BeHealthy.Domain;
using BeHealthy.Models;
using BeHealthy.Shared.Locales;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace BeHealthy.Components.Pages.Doctors;

public partial class Doctors : BasePage
{
    [Inject] IDoctorService _doctorService { get; set; } = default!;
    [Inject] IPatientService _patientsService { get; set; } = default!;
    [Inject] IPrivilegeService _privilegeService { get; set; } = default!;
    [Inject] NavigationManager _navigationManager { get; set; } = default!;
    [Inject] AuthenticationStateProvider _authenticationStateProvider { get; set; } = default!;

    private List<DoctorDto> _doctors { get; set; } = default!;

    private string _selectedView = "Card";
    private bool hasActionRights;
    private UserRole? _userRole;

    private PaginationState _paginationState = new();

    private bool showWizard = false;
    void ShowImportWizard() => showWizard = true;
    void HideImportWizard() => showWizard = false;

    protected override async Task OnInitializedAsync()
    {
        LoaderService.SetLoader(true);

        SetBreadcrumbs();

        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        _userRole = authState.User.GetUserRoleEnum();
        var userId = authState.User.GetUserId();

        await LoadDoctors(userId, _userRole);

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

    private async Task BulkCreateDoctors(List<DoctorForCreationDto> doctorForCreationDtos)
    {
        LoaderService.SetLoader(true);

        foreach (var doctor in doctorForCreationDtos)
        {
            var result = await _doctorService.AddDoctorAsync(doctor);
            if (!result.Success)
            {
                AlertModalStateService.Show(Resource.Error, result.ErrorMessage!);
                LoaderService.SetLoader(false);
                return;
            }
        }
        LoaderService.SetLoader(false);
        _navigationManager.Refresh(true);
    }

    private void ConfirmDelete(int doctorId)
    {
        ConfirmDeleteService.RequestDelete(async () =>
        {
            await _doctorService.DeleteDoctorAsync(doctorId);
            _navigationManager.Refresh(forceReload: true);
        });
    }
}
