using BeHealthy.Extensions;
using BeHealthy.Shared.Interfaces;
using BeHealthy.Shared.Models;
using BeHealthy.Shared.Models.Dtos.Appointment;
using BeHealthy.Shared.Models.Dtos.Doctor;
using BeHealthy.Shared.Models.Dtos.Patient;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace BeHealthy.Components.Pages.Appointments;

public partial class Index
{
    private IEnumerable<AppointmentDto> _appointments = default!;

    [Inject] IAppointmentService _appointmentService { get; set; } = default!;
    [Inject] IUserService _userService { get; set; } = default!;
    [Inject] IDoctorService _doctorService { get; set; } = default!;
    [Inject] INurseService _nurseService { get; set; } = default!;
    [Inject] IPatientService _patientService { get; set; } = default!;
    [Inject] NavigationManager _navigationManager { get; set; } = default!;
    [Inject] AuthenticationStateProvider _authenticationStateProvider { get; set; } = default!;

    private List<DoctorDto>? _doctors { get; set; }
    private List<PatientDto>? _patients { get; set; }

    private AppointmentModal _appointmentModal { get; set; } = new();
    private string? _currentUserId;
    private bool _hasActionRights;

    private bool _isLoading = default;

    private PaginationState _paginationState = new();

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;
        _paginationState.ItemsPerPage = 10;

        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        _currentUserId = authState.User.GetUserId();

        var roleClaim = authState.User.GetUserRole();

        if (Enum.TryParse(typeof(UserRole), roleClaim, out var roleEnum) && roleEnum is UserRole userRole)
        {
            _hasActionRights = userRole == UserRole.Admin || userRole == UserRole.Doctor;

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

            await _appointmentService.UpdateAppointmentAsync(appointmentId, appointmentForUpdate);
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

            await _appointmentService.AddAppointmentAsync(appointmentForCreation);
        }
        _navigationManager.Refresh(forceReload: true);
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
        else
        {
            Console.WriteLine("BUT FOUND SHIT");
        }
    }

    private async Task DeleteAppointment(int appointmentId)
    {
        await _appointmentService.DeleteAppointmentAsync(appointmentId);
        _navigationManager.Refresh(forceReload: true);
    }
}
