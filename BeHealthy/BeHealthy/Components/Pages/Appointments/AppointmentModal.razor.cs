using BeHealthy.Application.Dtos.Appointment;
using BeHealthy.Application.Dtos.Doctor;
using BeHealthy.Application.Dtos.Patient;
using BeHealthy.Application.Extensions;
using BeHealthy.Application.Helpers;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Application.Validations.Appointments;
using BeHealthy.Domain;
using BeHealthy.Extensions;
using BeHealthy.Models;
using BeHealthy.Shared.Locales;
using Microsoft.AspNetCore.Components;

namespace BeHealthy.Components.Pages.Appointments;

public partial class AppointmentModal : BasePage
{
    [Parameter, EditorRequired]
    public int? AppointmentId { get; set; }

    [Parameter]
    public EventCallback<(AppointmentDto, int?)> OnFormSubmit { get; set; }

    [SupplyParameterFromForm]
    private AppointmentDto _appointmentDto { get; set; } = new();

    [Parameter]
    public List<DoctorDto> Doctors { get; set; } = default!;

    [Parameter]
    public List<PatientDto> Patients { get; set; } = default!;

    [Parameter]
    public string CurrentUserId { get; set; } = default!;

    [Parameter]
    public UserRole? Role { get; set; }

    [Inject]
    private IRoomService _roomService { get; set; } = default!;

    [Inject]
    private IAppSettingsService _appSettingsService { get; set; } = default!;

    [Inject]
    private INurseService _nurseService { get; set; } = default!;

    [Inject]
    private IAppointmentService _appointmentService { get; set; } = default!;

    private List<SelectItem> _doctorsSelect = new();
    private List<SelectItem> _patientsSelect = new();
    private List<SelectItem> _roomsSelect = new();
    private List<SelectItem> _nursesSelect = new();

    private bool LockDoctorsDropdown => Role == UserRole.Doctor;
    private bool LockPatientsDropdown => Role == UserRole.Patient;

    private bool _show;
    private bool _isEdit => AppointmentId.HasValue;
    private bool _showRooms;
    private bool _showNurses;

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

        _appointmentDto.AppointmentDate = DateOnly.FromDateTime(DateTime.Today);

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
    }

    protected override async Task OnParametersSetAsync()
    {
        if (AppointmentId.HasValue && AppointmentId.Value > 0)
            _appointmentDto = await _appointmentService.GetAppointmentByIdAsync(AppointmentId.Value) ?? new();

        if (Role == UserRole.Doctor)
        {
            _appointmentDto.DoctorId = Doctors.FirstOrDefault(w => w.UserId == CurrentUserId)!.Id;
        }

        if (Role == UserRole.Patient)
        {
            _appointmentDto.PatientId = Patients.FirstOrDefault(w => w.UserId == CurrentUserId)!.Id;
        }
    }

    protected async Task GetAppSettings()
    {
        var keys = new[] { "AppointmentRequiresRoom", "NurseIsRequiredForAppointment" }.ToList();
        var settings = await _appSettingsService.GetMassAppSettingsAsync(keys);

        var nurseSetting = settings.FirstOrDefault(s => s.Key == "NurseIsRequiredForAppointment");
        var requireRoomSetting = settings.FirstOrDefault(s => s.Key == "AppointmentRequiresRoom");

        _showNurses = nurseSetting?.GetBooleanValue() ?? false;
        _showRooms = requireRoomSetting?.GetBooleanValue() ?? false;
    }

    public void Open() => _show = true;

    public void Close() => _show = false;

    public async Task HandleSaveClick()
    {
        _validationComponent?.ClearErrors();

        var validator = new AppointmentDtoValidator(_showNurses, _showRooms);
        var validationResult = await validator.ValidateAsync(_appointmentDto);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.GetErrorsGroupedByProperty();
            _validationComponent?.DisplayErrors(errors);
        }
        else
        {
            await OnFormSubmit.InvokeAsync((_appointmentDto, AppointmentId));
        }
    }

    public static IEnumerable<SelectItem> GetReasons()
    {
        var appointmentReasons = Enum.GetValues(typeof(AppointmentReason))
         .Cast<AppointmentReason>()
         .Select(reason => new SelectItem
         {
             Value = (int)reason,
             Text = reason.ToLocalizedString(),
         })
         .ToList();

        return appointmentReasons;
    }
}
