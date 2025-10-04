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
    [Inject] NavigationManager _navigationManager { get; set; } = default!;
    [Inject] AuthenticationStateProvider _authenticationStateProvider { get; set; } = default!;

    private List<DoctorSimpleDto>? _doctors { get; set; }
    private List<PatientSimpleDto>? _patients { get; set; }

    private AppointmentModal _appointmentModal { get; set; } = new();

    private string? _currentUserId;
    private bool hasActionRights;
    private bool hasEditRight;
    private bool hasDeleteRight;
    private int? _appointmentId;

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

    private async Task HandleAppointmentFormSubmission((AppointmentDto, int?) submission)
    {
        var (appointmentDto, appointmentId) = submission;
        _appointmentModal.Close();

        if (appointmentId is not null)
        {
            appointmentDto.Id = appointmentId.Value;

            var appointmentForUpdate = appointmentDto.MapToUpdateDto();
            await UpdateAppointmentAsync(appointmentForUpdate);
        }
        else
        {
            var appointmentForCreate = appointmentDto.MapToCreationDto();
            await CreateAppointmentAsync(appointmentForCreate);
        }
    }

    private async Task LoadDoctors()
    {
        _doctors = (await _doctorService.GetAllDoctorsSimpleAsync()).ToList();

        if (userRole == UserRole.Doctor)
        {
            _doctors = _doctors.Where(d => d.UserId.ToString() == _currentUserId).ToList();
        }
    }

    private async Task LoadPatients()
    {
        _patients = (await _patientService.GetAllPatientsSimpleAsync()).ToList();
    }

    private void EditAppointment(int appointmentId)
    {
        _appointmentId = appointmentId;
        _appointmentModal.Open();
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
        return false;
        //return await _privilegeService.HasPrivilegeAsync(role, privilege);
    }

    private async Task CreateAppointmentAsync(AppointmentCreateDto AppointmentCreateDto, bool fromBulkCreation = false)
    {
        var response = await _appointmentService.AddAppointmentAsync(AppointmentCreateDto);
        if (HandleServiceResponse(response))
        {
            if (!fromBulkCreation)
                _navigationManager.Refresh(true);
        }
    }

    private async Task UpdateAppointmentAsync(AppointmentUpdateDto appointmentForUpdateDto)
    {
        var response = await _appointmentService.UpdateAppointmentAsync(appointmentForUpdateDto);

        if (HandleServiceResponse(response))
            _navigationManager.Refresh(true); 
    }

    private async Task BulkCreateAppointments((List<AppointmentCreateDto> AppointmentCreateDtos, bool UseValidation) result)
    {
        var useValidation = result.UseValidation;
        foreach (var appointment in result.AppointmentCreateDtos)
        {
            await CreateAppointmentAsync(appointment, true);
        }
    }
}
