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

    protected override async Task OnInitializedAsync()
    {
        LoaderService.SetLoader(true);

        SetBreadcrumbs();
        _paginationState.ItemsPerPage = 10;

        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        _currentUserId = authState.User.GetUserId();

        userRole = authState.User.GetUserRoleEnum();

        switch (userRole)
        {
            case UserRole.Doctor when _currentUserId is not null:
                await LoadDoctorAppointments(_currentUserId);
                break;
            case UserRole.Patient when _currentUserId is not null:
                await LoadPatientAppointments(_currentUserId);
                break;
            default:
                await LoadAppointments();
                break;
        }

        //hasEditRight = await PrivilegeStateService.HasPrivilegeAsync(PrivilegeName.EditAppointments);
        //hasDeleteRight = await PrivilegeStateService.HasPrivilegeAsync(PrivilegeName.DeleteAppointments);
        hasActionRights = hasEditRight || hasDeleteRight;

        await LoadDoctors();
        await LoadPatients();

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

    private void OnPageSizeChanged(ChangeEventArgs e)
    {
        if (e.Value is not null)
        {
            _paginationState.ItemsPerPage = int.Parse((string)e.Value);
        }
    }

    private async Task HandleAppointmentFormSubmission((AppointmentDto, bool, int) submission)
    {
        var (appointmentDto, isEdit, appointmentId) = submission;
        _appointmentModal.Close();
        ServiceResponse result;

        if (isEdit)
        {
            appointmentDto.Id = appointmentId;

            var appointmentForUpdate = appointmentDto.MapToUpdateDto();

            result = await _appointmentService.UpdateAppointmentAsync(appointmentId, appointmentForUpdate);
        }
        else
        {
            var appointmentForCreation = appointmentDto.MapToCreationDto();

            result = await _appointmentService.AddAppointmentAsync(appointmentForCreation);
        }

        if (!result.Success)
        {
            _alert.ShowFailed(result.ErrorMessage!);
        }
        else
        {
            await ToastrStateService.ShowSuccess(Resource.Success, 1000);
            _navigationManager.Refresh(true);
        }
    }

    private async Task LoadAppointments()
    {
        _appointments = (await _appointmentService.GetAllAppointmentsAsync()).ToList();
    }

    private async Task LoadPatientAppointments(string userId)
    {
        _appointments = (await _patientService.GetPatientAppointmentsByUserIdAsync(userId)).ToList();
    }

    private async Task LoadDoctorAppointments(string userId)
    {
        _appointments = (await _doctorService.GetDoctorAppointmentsByUserIdAsync(userId)).ToList();
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
}
