using BeHealthy.Shared.Models.Dtos.Appointment;
using BeHealthy.Shared.Models.Dtos.Doctor;
using BeHealthy.Shared.Models.Dtos.Patient;
using Microsoft.AspNetCore.Components;


namespace BeHealthy.Client.Components;

public partial class CreateAppointmentModal
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
    }

    public void Close()
    {
        _show = false;
    }

    public async Task HandleSaveClick()
    {
        await OnFormSubmit.InvokeAsync((_appointmentForCreationDto, _isEdit, _appointmentId));
    }
}
