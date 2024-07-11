using BeHealthy.Client.Services;
using BeHealthy.Client.Services.Interfaces;
using BeHealthy.Components.Components;
using BeHealthy.Shared.Models.Dtos.Appointment;
using BeHealthy.Shared.Models.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BeHealthy.Client.Pages.Appointments;

public partial class Index
{
    private IEnumerable<AppointmentDto> _appointments = default!;

    [Inject] IAppointmentService _appointmentService { get; set; } = default!;
    [Inject] IUserService _userService { get; set; } = default!;
    [Inject] NavigationManager _navigationManager { get; set; } = default!;
    [Inject] AuthenticationStateProvider _authenticationStateProvider { get; set; } = default!;

    private List<ApplicationUser>? _doctors { get; set; }
    private List<ApplicationUser>? _patients { get; set; }

    private CreateAppointmentModal _createAppointmentModal { get; set; } = new();
    private string? _currentUserId;
    private bool _hasActionRights;

    protected override async Task OnInitializedAsync()
    {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        _currentUserId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        _hasActionRights = user.IsInRole("Admin") || user.IsInRole("Doctor");

        if (user.IsInRole("Admin"))
        {
            await LoadAppointments();
        }
        else if (user.IsInRole("Doctor") && _currentUserId is not null)
        {
            await LoadAppointmentsForDoctor(_currentUserId);
        }
        else if (user.IsInRole("Patient") && _currentUserId is not null)
        {
            await LoadAppointmentsForPatient(_currentUserId);
        }
       
        await LoadDoctors();
        await LoadPatients();
    }

    private async Task HandleAppointmentFormSubmission((AppointmentForCreationDto, bool, int) submission)
    {
        var (appointmentForCreation, isEdit, appointmentId) = submission;
        _createAppointmentModal.Close();
        if (isEdit)
        {
            var appointmentForUpdate = new AppointmentForUpdateDto
            {
                Id = appointmentId,
                DoctorId = appointmentForCreation.DoctorId,
                PatientId = appointmentForCreation.PatientId,
                Notes = appointmentForCreation.Notes,
                AppointmentDate = appointmentForCreation.AppointmentDate
            };

            await _appointmentService.UpdateAppointmentAsync(appointmentId, appointmentForUpdate);
        }
        else
        {
            await _appointmentService.AddAppointmentAsync(appointmentForCreation);
        }
        _navigationManager.Refresh(forceReload: true);
    }

    private async Task LoadAppointments()
    {
        _appointments = await _appointmentService.GetAllAppointmentsAsync();
    }

    private async Task LoadAppointmentsForDoctor(string doctorId)
    {
        _appointments = await _appointmentService.GetAppointmentsByDoctorIdAsync(doctorId);
    }

    private async Task LoadAppointmentsForPatient(string patientId)
    {
        _appointments = await _appointmentService.GetAppointmentsByPatientIdAsync(patientId);
    }

    private async Task LoadDoctors()
    {
        _doctors = (await _userService.GetAllDoctorsAsync()).ToList();
    }

    private async Task LoadPatients()
    {
        _patients = (await _userService.GetAllPatientsAsync()).ToList();
    }

    private void EditAppointment(int appointmentId)
    {
        var appointment = _appointments.FirstOrDefault(a => a.Id == appointmentId);
        if (appointment != null)
        {
            _createAppointmentModal.OpenForEdit(appointment);
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
