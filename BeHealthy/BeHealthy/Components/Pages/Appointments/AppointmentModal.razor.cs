using BeHealthy.Application.Dtos.Appointment;
using BeHealthy.Application.Dtos.Doctor;
using BeHealthy.Application.Dtos.Patient;
using BeHealthy.Application.Extensions;
using BeHealthy.Domain;
using BeHealthy.Locales;
using BeHealthy.Models;
using Microsoft.AspNetCore.Components;

namespace BeHealthy.Components.Pages.Appointments;

public partial class AppointmentModal
{
    [Parameter]
    public EventCallback<(AppointmentDto, bool, int)> OnFormSubmit { get; set; }

    [SupplyParameterFromForm]
    private AppointmentDto _appointmentDto { get; set; } = new();

    [Parameter]
    public List<DoctorDto> Doctors { get; set; } = default!;
    [Parameter]
    public List<PatientDto> Patients { get; set; } = default!;

    [Parameter]
    public string? Role { get; set; }

    private List<SelectItem> _doctorsSelect = new List<SelectItem>();
    private List<SelectItem> _patientsSelect = new List<SelectItem>();

    private bool LockDoctorsDropdown => Role == UserRole.Doctor.GetDisplayName();

    private bool _show;
    private bool _isEdit;
    private int _appointmentId;

    private int AppointmentHour { get; set; } = 0;
    private int AppointmentMinute { get; set; } = 0;

    protected override void OnParametersSet()
    {
        _doctorsSelect = Doctors.Select(s => new SelectItem
        {
            Value = s.Id,
            Text = s.FullName
        }).ToList();
        _doctorsSelect.Insert(0, new SelectItem { Value = 0, Text = Resource.PleaseSelect });

        _patientsSelect = Patients.Select(s => new SelectItem
        {
            Value = s.Id,
            Text = s.FullName
        }).ToList();
        _patientsSelect.Insert(0, new SelectItem { Value = 0, Text = Resource.PleaseSelect });
    }

    public void Open()
    {
        _show = true;
        _isEdit = false;
        _appointmentDto = new();
        _appointmentId = 0;
        _appointmentDto.PatientId = Patients.First().Id;
        _appointmentDto.DoctorId = Doctors.First().Id;
        _appointmentDto.AppointmentDate = DateTime.Now;
    }

    public void OpenForEdit(AppointmentDto appointment)
    {
        _show = true;
        _isEdit = true;
        _appointmentDto.DoctorId = appointment.DoctorId;
        _appointmentDto.PatientId = appointment.PatientId;
        _appointmentDto.Notes = appointment.Notes;
        _appointmentDto.AppointmentDate = appointment.AppointmentDate;
        _appointmentDto.Duration = appointment.Duration;
        _appointmentId = appointment.Id;
        AppointmentHour = appointment.AppointmentDate.Hour;
        AppointmentMinute = appointment.AppointmentDate.Minute;
        _appointmentDto.Status = appointment.Status;
    }

    public void Close()
    {
        _show = false;
    }

    public async Task HandleSaveClick()
    {
        var appointmentTime = new TimeSpan(AppointmentHour, AppointmentMinute, 0);
        _appointmentDto.AppointmentDate =
            new DateTime(
                _appointmentDto.AppointmentDate.Year,
                _appointmentDto.AppointmentDate.Month,
                _appointmentDto.AppointmentDate.Day,
                appointmentTime.Hours,
                appointmentTime.Minutes,
                0);

        await OnFormSubmit.InvokeAsync((_appointmentDto, _isEdit, _appointmentId));
    }
}
