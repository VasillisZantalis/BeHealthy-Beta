using BeHealthy.Shared.Models.Dtos.Appointment;
using BeHealthy.Shared.Models.Dtos.Doctor;
using BeHealthy.Shared.Models.Dtos.Patient;
using Microsoft.AspNetCore.Components;


namespace BeHealthy.Client.Components;

public partial class AppointmentModal
{
    [Parameter]
    public EventCallback<(AppointmentForCreationDto, bool, int)> OnFormSubmit { get; set; }

    [SupplyParameterFromForm]
    private AppointmentForCreationDto _appointmentForCreationDto { get; set; } = new();

    [Parameter]
    public List<DoctorDto> Doctors { get; set; } = default!;
    [Parameter]
    public List<PatientDto> Patients { get; set; } = default!;

    private bool _show;
    private bool _isEdit;
    private int _appointmentId;

    private int AppointmentHour { get; set; } = 0;
    private int AppointmentMinute { get; set; } = 0;

    public void Open()
    {
        _show = true;
        _isEdit = false;
        _appointmentForCreationDto = new();
        _appointmentId = 0;
        _appointmentForCreationDto.PatientId = Patients?.FirstOrDefault()?.Id;
        _appointmentForCreationDto.DoctorId = Doctors?.FirstOrDefault()?.Id;
        _appointmentForCreationDto.AppointmentDate = DateTime.Now;
    }

    public void OpenForEdit(AppointmentDto appointment)
    {
        _show = true;
        _isEdit = true;
        _appointmentForCreationDto = new AppointmentForCreationDto
        {
            DoctorId = appointment.DoctorId,
            PatientId = appointment.PatientId,
            Notes = appointment.Notes,
            AppointmentDate = appointment.AppointmentDate,
        };
        _appointmentId = appointment.Id;
        AppointmentHour = appointment.AppointmentDate.Hour;
        AppointmentMinute = appointment.AppointmentDate.Minute;
    }

    public void Close()
    {
        _show = false;
    }

    public async Task HandleSaveClick()
    {
        var appointmentTime = new TimeSpan(AppointmentHour, AppointmentMinute, 0);
        _appointmentForCreationDto.AppointmentDate = 
            new DateTime(
                _appointmentForCreationDto.AppointmentDate.Year,
                _appointmentForCreationDto.AppointmentDate.Month,
                _appointmentForCreationDto.AppointmentDate.Day,
                appointmentTime.Hours,
                appointmentTime.Minutes,
                0);

        await OnFormSubmit.InvokeAsync((_appointmentForCreationDto, _isEdit, _appointmentId));
    }
}
