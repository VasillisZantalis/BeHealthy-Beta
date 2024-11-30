using BeHealthy.Application.Dtos.Appointment;
using BeHealthy.Application.Dtos.Doctor;
using BeHealthy.Application.Dtos.Patient;
using BeHealthy.Application.Helpers;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Domain;
using BeHealthy.Models;
using BeHealthy.Shared.Locales;
using Microsoft.AspNetCore.Components;

namespace BeHealthy.Components.Pages.Appointments;

public partial class AppointmentModal : BasePage
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
    public UserRole? Role { get; set; }

    private List<SelectItem> _doctorsSelect = new();
    private List<SelectItem> _patientsSelect = new();
    private List<SelectItem> _roomsSelect = new();
    private List<SelectItem> _nursesSelect = new();

    //private List<SelectItem> _statusDropdownItems = new();
    //private List<SelectItem> _reasonDropdownItems = new();

    [Inject]
    private IRoomService _roomService { get; set; } = default!;

    [Inject]
    private IAppSettingsService _appSettingsService { get; set; } = default!;

    [Inject]
    private INurseService _nurseService { get; set; } = default!;

    private bool LockDoctorsDropdown => Role == UserRole.Doctor;

    private bool _show;
    private bool _isEdit;
    private int _appointmentId;
    private bool _showRooms;
    private bool _showNurses;

    private int AppointmentHour { get; set; } = 0;
    private int AppointmentMinute { get; set; } = 0;

    private ValidationComponent? _validationComponent;

    protected override async Task OnInitializedAsync()
    {
        LoaderService.SetLoader(true);

        var rooms = (await _roomService.GetAllRoomsAsync()).ToList();
        var nurses = (await _nurseService.GetAllNursesAsync()).ToList();

        await GetAppSettings();
       
        _roomsSelect = rooms.Select(s => new SelectItem
        {
            Value = s.Id,
            Text = s.Name,
        }).ToList();
        _roomsSelect.Insert(0, new SelectItem { Text = Resource.PleaseSelect, Value = 0 });

        _nursesSelect = nurses.Select(s => new SelectItem
        {
            Value = s.Id,
            Text = s.FullName,
        }).ToList();
        _nursesSelect.Insert(0, new SelectItem { Text = Resource.PleaseSelect, Value = 0 });

        LoaderService.SetLoader(false);
    }

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

        //_statusDropdownItems = Enum.GetValues(typeof(AppointmentStatus))
        //    .Cast<AppointmentStatus>()
        //    .Where(status => status != AppointmentStatus.Scheduled)
        //    .Select(status => new SelectItem
        //    {
        //        Value = (int)status,
        //        Text = status.ToLocalizedString(),
        //        Selected = _appointmentDto.Status == status
        //    })
        //    .ToList();

        //_reasonDropdownItems = Enum.GetValues(typeof(AppointmentReason))
        //    .Cast<AppointmentReason>()
        //    .Select(status => new SelectItem
        //    {
        //        Value = (int)status,
        //        Text = status.ToLocalizedString()
        //    })
        //    .ToList();
    }

    protected async Task GetAppSettings()
    {
        var keys = new[] { "RequiredRoomsInAppointments", "NurseIsRequiredForAppointment" }.ToList();
        var settings = await _appSettingsService.GetMassAppSettingsAsync(keys);

        var nurseSetting = settings.FirstOrDefault(s => s.Key == "NurseIsRequiredForAppointment");
        var requireRoomSetting = settings.FirstOrDefault(s => s.Key == "RequiredRoomsInAppointments");

        if (requireRoomSetting != null)
        {
            _showRooms = AppSettingsConverterHelper.GetBooleanValue(requireRoomSetting);
        }

        if (nurseSetting != null)
        {
            _showNurses = AppSettingsConverterHelper.GetBooleanValue(nurseSetting);
        }

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
        _appointmentDto.RoomId = appointment.RoomId;
        _appointmentDto.NurseId = appointment.NurseId;
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
        _validationComponent?.ClearErrors();
        var errors = new Dictionary<string, List<string>>();

        if (_showNurses && _appointmentDto.NurseId is null)
        {
            errors.Add(nameof(_appointmentDto.NurseId), 
                new() { Resource.Required });
        }

        if (_showRooms && _appointmentDto.RoomId is null)
        {
            errors.Add(nameof(_appointmentDto.RoomId), 
                new() { Resource.Required });
        }

        if (errors.Any())
        {
            _validationComponent?.DisplayErrors(errors);
        }
        else
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
}
