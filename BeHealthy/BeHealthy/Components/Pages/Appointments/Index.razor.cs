using BeHealthy.Application.Dtos.Appointment;
using BeHealthy.Application.Dtos.Common;
using BeHealthy.Application.Dtos.Doctor;
using BeHealthy.Application.Dtos.Patient;
using BeHealthy.Application.Extensions;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Components.Shared.Controls;
using BeHealthy.Components.Shared.Modals;
using BeHealthy.Domain;
using BeHealthy.Shared.Locales;
using BeHealthy.States;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.QuickGrid;
using System.Data;

namespace BeHealthy.Components.Pages.Appointments;

public partial class Index
{
    private IEnumerable<AppointmentDto> _appointments = default!;

    [Inject] IAppointmentService _appointmentService { get; set; } = default!;
    [Inject] IUserService _userService { get; set; } = default!;
    [Inject] IDoctorService _doctorService { get; set; } = default!;
    [Inject] INurseService _nurseService { get; set; } = default!;
    [Inject] IPatientService _patientService { get; set; } = default!;
    [Inject] PrivilegeStateService _privilegeStateService { get; set; } = default!;
    [Inject] NavigationManager _navigationManager { get; set; } = default!;
    [Inject] AuthenticationStateProvider _authenticationStateProvider { get; set; } = default!;

    private List<DoctorDto>? _doctors { get; set; }
    private List<PatientDto>? _patients { get; set; }
    private ConfirmDeleteModal _confirmDeleteModal = new();

    private AppointmentModal _appointmentModal { get; set; } = new();
    private Alert _alert = new();
    private Toastr _toastr = new();

    private string? _currentUserId;
    private bool hasActionRights;
    private bool hasEditRight;
    private bool hasDeleteRight;

    private int deleteItemId;

    private bool _isLoading = default;
    private string? roleClaim;

    private PaginationState _paginationState = new();

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;
        _paginationState.ItemsPerPage = 10;

        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        _currentUserId = authState.User.GetUserId();

        roleClaim = authState.User.GetUserRole();

        if (Enum.TryParse(typeof(UserRole), roleClaim, out var roleEnum) && roleEnum is UserRole userRole)
        {
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

            hasEditRight = await _privilegeStateService.HasPrivilegeAsync("CanEditAppointment");
            hasDeleteRight = await _privilegeStateService.HasPrivilegeAsync("CanDeleteAppointment");
            hasActionRights = hasEditRight || hasDeleteRight;
        }

        await LoadDoctors();
        await LoadPatients();
        _isLoading = false;
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
            var appointmentForUpdate = new AppointmentForUpdateDto
            {
                Id = appointmentId,
                DoctorId = appointmentDto.DoctorId,
                PatientId = appointmentDto.PatientId,
                Notes = appointmentDto.Notes,
                AppointmentDate = appointmentDto.AppointmentDate,
                Status = appointmentDto.Status,
                Reason = appointmentDto.Reason,
                Duration = appointmentDto.Duration
            };

            result = await _appointmentService.UpdateAppointmentAsync(appointmentId, appointmentForUpdate);
        }
        else
        {
            var appointmentForCreation = new AppointmentForCreationDto
            {
                DoctorId = appointmentDto.DoctorId,
                PatientId = appointmentDto.PatientId,
                Notes = appointmentDto.Notes,
                AppointmentDate = appointmentDto.AppointmentDate,
                Reason = appointmentDto.Reason,
                Status = AppointmentStatus.Scheduled,
                Duration = appointmentDto.Duration
            };

            result = await _appointmentService.AddAppointmentAsync(appointmentForCreation);
        }

        if (!result.Success)
        {
            _alert.ShowFailed(result.ErrorMessage!);
        }
        else
        {
            await _toastr.ShowSuccess(Resource.Success);
            _navigationManager.Refresh(forceReload: true);
        }
    }

    private async Task LoadAppointments()
    {
        _appointments = await _appointmentService.GetAllAppointmentsAsync();
    }

    private async Task LoadPatientAppointments(string userId)
    {
        _appointments = await _patientService.GetPatientAppointmentsByUserIdAsync(userId);
    }

    private async Task LoadDoctorAppointments(string userId)
    {
        _appointments = await _doctorService.GetDoctorAppointmentsByUserIdAsync(userId);
    }

    private async Task LoadDoctors()
    {
        _doctors = (await _doctorService.GetAllDoctorsAsync()).ToList();

        if (roleClaim == UserRole.Doctor.GetDisplayName())
        {
            _doctors = _doctors.Where(d => d.UserId.ToString() == _currentUserId).ToList();
        }
    }

    private async Task LoadPatients()
    {
        _patients = (await _patientService.GetAllPatientsAsync()).ToList();
    }

    private void EditAppointment(int appointmentId)
    {
        var appointment = _appointments.FirstOrDefault(a => a.Id == appointmentId);
        if (appointment != null)
        {
            _appointmentModal.OpenForEdit(appointment);
        }
    }

    private void ConfirmDelete(int id)
    {
        deleteItemId = id;
        _confirmDeleteModal.HandleOpen();
    }

    private async Task OnDeleteConfirmed(bool confirmed)
    {
        if (confirmed)
        {
            await _appointmentService.DeleteAppointmentAsync(deleteItemId);
            _navigationManager.Refresh(forceReload: true);
        }
    }
}
