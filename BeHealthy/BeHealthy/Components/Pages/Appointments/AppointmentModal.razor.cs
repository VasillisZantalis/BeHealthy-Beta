using BeHealthy.Application.Dtos.Appointment;
using BeHealthy.Application.Dtos.Doctor;
using BeHealthy.Application.Dtos.Patient;
using BeHealthy.Application.Extensions;
using BeHealthy.Application.Helpers;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Application.Validations.Appointments;
using BeHealthy.Components.Shared.Modals.Base;
using BeHealthy.Domain;
using BeHealthy.Extensions;
using BeHealthy.Models;
using BeHealthy.Shared.Locales;
using BeHealthy.States;
using Microsoft.AspNetCore.Components;

namespace BeHealthy.Components.Pages.Appointments;

public partial class AppointmentModal : ModalBase
{
    [Parameter, EditorRequired]
    public int? AppointmentId { get; set; }

    [Parameter]
    public EventCallback<(AppointmentDto, int?)> OnFormSubmit { get; set; }

    [SupplyParameterFromForm]
    private AppointmentDto appointmentDto { get; set; } = new();

    [Parameter]
    public List<DoctorSimpleDto> Doctors { get; set; } = default!;

    [Parameter]
    public List<PatientSimpleDto> Patients { get; set; } = default!;

    [Parameter]
    public string CurrentUserId { get; set; } = default!;

    [Parameter]
    public UserRole? Role { get; set; }

    [Inject]
    private IRoomService roomService { get; set; } = default!;

    [Inject]
    private IAppSettingsService appSettingsService { get; set; } = default!;

    [Inject]
    private INurseService NurseService { get; set; } = default!;

    [Inject]
    private IAppointmentService AppointmentService { get; set; } = default!;

    [Inject] LoaderServiceState LoaderService { get; set; } = default!;

    private List<SelectItem> doctorsSelect = new();
    private List<SelectItem> patientsSelect = new();
    private List<SelectItem> roomsSelect = new();
    private List<SelectItem> nursesSelect = new();

    private bool LockDoctorsDropdown => Role == UserRole.Doctor;
    private bool LockPatientsDropdown => Role == UserRole.Patient;

    private bool show;
    private bool isEdit => AppointmentId.HasValue;
    private bool showRooms;
    private bool showNurses;

    private ValidationComponent? validationComponent;

    protected override async Task OnInitializedAsync()
    {
        LoaderService.SetLoader(true);

        var rooms = (await roomService.GetAllRoomsAsync()).ToList();
        var nurses = (await NurseService.GetAllNursesAsync()).ToList();

        await GetAppSettings();
       
        roomsSelect = rooms.Select(s => new SelectItem
        {
            Value = s.Id,
            Text = s.Name,
        }).ToList();
        roomsSelect.Insert(0, new SelectItem { Text = Resource.PleaseSelect, Value = 0 });

        nursesSelect = nurses.Select(s => new SelectItem
        {
            Value = s.Id,
            Text = s.FullName,
        }).ToList();
        nursesSelect.Insert(0, new SelectItem { Text = Resource.PleaseSelect, Value = 0 });


        // Even thought we set currect hours, still are converted to UTC
        // Leave it like this for now

        var now = DateTime.Now;
        appointmentDto.AppointmentDate = DateOnly.FromDateTime(now);
        appointmentDto.AppointmentStartTime = TimeOnly.FromDateTime(now);
        appointmentDto.AppointmentEndTime = TimeOnly.FromDateTime(now.AddHours(1));

        LoaderService.SetLoader(false);
    }

    protected override void OnParametersSet()
    {
        doctorsSelect = Doctors.Select(s => new SelectItem
        {
            Value = s.Id,
            Text = s.FullName
        }).ToList();
        doctorsSelect.Insert(0, new SelectItem { Value = 0, Text = Resource.PleaseSelect });

        patientsSelect = Patients.Select(s => new SelectItem
        {
            Value = s.Id,
            Text = s.FullName
        }).ToList();
        patientsSelect.Insert(0, new SelectItem { Value = 0, Text = Resource.PleaseSelect });
    }

    protected override async Task OnParametersSetAsync()
    {
        if (AppointmentId.HasValue && AppointmentId.Value > 0)
            appointmentDto = await AppointmentService.GetAppointmentByIdAsync(AppointmentId.Value) ?? new();

        if (Role == UserRole.Doctor)
        {
            appointmentDto.DoctorId = Doctors.FirstOrDefault(w => w.UserId == CurrentUserId)!.Id;
        }

        if (Role == UserRole.Patient)
        {
            appointmentDto.PatientId = Patients.FirstOrDefault(w => w.UserId == CurrentUserId)!.Id;
        }
    }

    protected async Task GetAppSettings()
    {
        var keys = new[] { "AppointmentRequiresRoom", "NurseIsRequiredForAppointment" }.ToList();
        var settings = await appSettingsService.GetMassAppSettingsAsync(keys);

        var nurseSetting = settings.FirstOrDefault(s => s.Key == "NurseIsRequiredForAppointment");
        var requireRoomSetting = settings.FirstOrDefault(s => s.Key == "AppointmentRequiresRoom");

        showNurses = nurseSetting?.GetBooleanValue() ?? false;
        showRooms = requireRoomSetting?.GetBooleanValue() ?? false;
    }

    public async Task HandleSaveClick()
    {
        validationComponent?.ClearErrors();

        var validator = new AppointmentDtoValidator(showNurses, showRooms);
        var validationResult = await validator.ValidateAsync(appointmentDto);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.GetErrorsGroupedByProperty();
            validationComponent?.DisplayErrors(errors);
        }
        else
        {
            await OnFormSubmit.InvokeAsync((appointmentDto, AppointmentId));
            Close();
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
