using BeHealthy.Application.Dtos.Appointment;
using BeHealthy.Application.Dtos.Common;
using BeHealthy.Application.Dtos.Doctor;
using BeHealthy.Application.Dtos.Patient;
using BeHealthy.Application.Extensions;
using BeHealthy.Application.Mappings;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Common;
using BeHealthy.Components.Shared.Controls;
using BeHealthy.Domain;
using BeHealthy.Models;
using BeHealthy.Shared.Locales;
using BeHealthy.Shared.Parameters;
using BeHealthy.States;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.QuickGrid;
using System.Data;

namespace BeHealthy.Components.Pages.Appointments;

public partial class Appointments : BasePage
{
    private List<AppointmentDto> _appointments = default!;

    [Inject] IAppointmentService _appointmentService { get; set; } = default!;
    [Inject] IDoctorService _doctorService { get; set; } = default!;
    [Inject] INurseService _nurseService { get; set; } = default!;
    [Inject] IPatientService _patientService { get; set; } = default!;
    [Inject] IPrivilegeService _privilegeService { get; set; } = default!;
    [Inject] NavigationManager _navigationManager { get; set; } = default!;
    [Inject] AuthenticationStateProvider _authenticationStateProvider { get; set; } = default!;

    private List<DoctorDto>? _doctors { get; set; }
    private List<PatientDto>? _patients { get; set; }

    private AppointmentModal _appointmentModal { get; set; } = new();
    private Alert _alert = new();

    private string? _currentUserId;
    private bool hasActionRights;
    private bool hasEditRight;
    private bool hasDeleteRight;

    private UserRole? userRole;

    private PaginationState _paginationState = new();

    private bool showWizard = false;
    void ShowImportWizard() => showWizard = true;
    void HideImportWizard() => showWizard = false;

    protected override async Task OnInitializedAsync()
    {
        LoaderService.SetLoader(true);

        SetBreadcrumbs();

        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        _currentUserId = authState.User.GetUserId();

        userRole = authState.User.GetUserRoleEnum();

        switch (userRole, _currentUserId)
        {
            case (UserRole.Doctor, not null):
                _appointments = (await _doctorService.GetDoctorAppointmentsByUserIdAsync(_currentUserId)).ToList();
                break;

            case (UserRole.Patient, not null):
                _appointments = (await _patientService.GetPatientAppointmentsByUserIdAsync(_currentUserId)).ToList();
                break;

            default:
                _appointments = (await _appointmentService.GetAllAppointmentsAsync()).ToList();
                break;
        }

        await GetUserPrivilege(userRole!.Value);

        hasActionRights = hasEditRight || hasDeleteRight;

        await LoadDoctors();
        await LoadPatients();

        _paginationState.ItemsPerPage = _appointments.Count;

        LoaderService.SetLoader(false);
    }

    private void SetBreadcrumbs()
    {
        Breadcrumbs.SetBreadcrumbs(new List<Breadcrumb>()
        {
            new Breadcrumb(){ Text = Resource.Dashboard, Link = RoutingEndpoints.HOME_PAGE, Active = false },
            new Breadcrumb(){ Text = Resource.Appointments, Link = string.Empty, Active = true },
        });
    }

    private async Task HandleAppointmentFormSubmission((AppointmentDto, bool, int) submission)
    {
        var (appointmentDto, isEdit, appointmentId) = submission;
        _appointmentModal.Close();

        if (isEdit)
        {
            appointmentDto.Id = appointmentId;

            var appointmentForUpdate = appointmentDto.MapToUpdateDto();

            await UpdateAppointmentAsync(appointmentId, appointmentForUpdate);
        }
        else
        {
            var appointmentForCreation = appointmentDto.MapToCreationDto();
            await CreateAppointmentAsync(appointmentForCreation);
        }
    }

    private async Task LoadDoctors()
    {
        _doctors = (await _doctorService.GetAllDoctorsAsync()).ToList();

        if (userRole == UserRole.Doctor)
        {
            _doctors = _doctors.Where(d => d.UserId.ToString() == _currentUserId).ToList();
        }
    }

    private async Task LoadPatients()
    {
        _patients = (await _patientService.GetAllPatientsAsync(new PatientSearchingParameters())).ToList();
    }

    private void EditAppointment(int appointmentId)
    {
        var appointment = _appointments.FirstOrDefault(a => a.Id == appointmentId);
        if (appointment != null)
        {
            _appointmentModal.OpenForEdit(appointment);
        }
    }

    private void ConfirmDelete(int appointmentId)
    {
        ConfirmDeleteService.RequestDelete(async () =>
        {
            await _appointmentService.DeleteAppointmentAsync(appointmentId);
            _navigationManager.Refresh(forceReload: true);
        });
    }

    private async Task GetUserPrivilege(UserRole userRole)
    {
        (hasEditRight, hasDeleteRight) = userRole switch
        {
            UserRole.Admin => (true, true),
            _ => (await GetPrivilege(userRole, PrivilegeName.EditAppointments),
                await GetPrivilege(userRole, PrivilegeName.DeleteAppointments))
        };
    }

    private async Task<bool> GetPrivilege(UserRole role, PrivilegeName privilege)
    {
        return await _privilegeService.HasPrivilegeAsync(role, privilege);
    }

    private async Task CreateAppointmentAsync(AppointmentForCreationDto appointmentForCreationDto)
    {
        var result = await _appointmentService.AddAppointmentAsync(appointmentForCreationDto);
        await HandleServiceResponse(result);
    }

    private async Task UpdateAppointmentAsync(int appointmentId, AppointmentForUpdateDto appointmentForUpdateDto)
    {
        var result = await _appointmentService.UpdateAppointmentAsync(appointmentId, appointmentForUpdateDto);

        await HandleServiceResponse(result);
    }

    private async Task BulkCreateAppointments(List<AppointmentForCreationDto> appointmentForCreationDtos)
    {
        foreach (var appointment in appointmentForCreationDtos)
        {
            await CreateAppointmentAsync(appointment);
        }
    }

    private async Task HandleServiceResponse(ServiceResponse response)
    {
        if (!response.Success)
        {
            AlertModalStateService.Show(null, response.ErrorMessage);
            return;
        }
        else
        {
            await ToastrStateService.ShowSuccess(Resource.Success, 1000);
            _navigationManager.Refresh(true);
        }
    }
}
