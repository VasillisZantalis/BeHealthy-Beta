using BeHealthy.Shared.Dtos.Appointment;
using BeHealthy.Shared.Dtos.Doctor;
using BeHealthy.Shared.Dtos.Patient;
using BeHealthy.Front.Extensions;
using BeHealthy.Front.Helpers;
using BeHealthy.Front.Mappings;
using BeHealthy.Front.Services.Interfaces;
using BeHealthy.Front.Components.Shared.Modals.Base;
using BeHealthy.Shared;
using BeHealthy.Shared.Common;
using BeHealthy.Front.Models;
using BeHealthy.Shared.Locales;
using Microsoft.AspNetCore.Components;

namespace BeHealthy.Front.Components.Pages.Appointments;

public partial class AppointmentModal : ModalBase
{
    [Parameter, EditorRequired]
    public int? AppointmentId { get; set; }

    [Parameter]
    public EventCallback<(AppointmentResponse, int?)> OnFormSubmit { get; set; }

    [SupplyParameterFromForm]
    private AppointmentResponse appointmentDto { get; set; } = new();

    [Parameter]
    public List<DoctorSimpleResponse> Doctors { get; set; } = default!;

    [Parameter]
    public List<PatientSimpleResponse> Patients { get; set; } = default!;

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

    [Inject]
    private FluentValidation.IValidator<AppointmentCreateRequest> CreateValidator { get; set; } = default!;

    [Inject]
    private FluentValidation.IValidator<AppointmentUpdateRequest> UpdateValidator { get; set; } = default!;

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
    private bool IsLoading = false;

    private ValidationComponent? validationComponent;

    protected override void OnInitialized()
    {
        var now = DateTime.Now;
        appointmentDto.AppointmentDate = DateOnly.FromDateTime(now);
        appointmentDto.AppointmentStartTime = TimeOnly.FromDateTime(now);
        appointmentDto.AppointmentEndTime = TimeOnly.FromDateTime(now.AddHours(1));
    }

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;

        var rooms = (await roomService.GetAllRoomsAsync()).ToList();
        var nurses = (await NurseService.GetAllNursesSimpleAsync()).ToList();

        await GetAppSettings();

        roomsSelect = rooms.Select(s => new SelectItem
        {
            Value = s.Id,
            Text = s.Name,
        }).ToList();

        nursesSelect = nurses.Select(s => new SelectItem
        {
            Value = s.Id,
            Text = s.FullName,
        }).ToList();


        // Even thought we set currect hours, still are converted to UTC
        // Leave it like this for now

        IsLoading = false;
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
        {
            appointmentDto = await AppointmentService.GetAppointmentByIdAsync(AppointmentId.Value) ?? new();
        }

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
        var keys = new[] { AppSettingKeys.AppointmentRequiresRoom, AppSettingKeys.NurseIsRequiredForAppointment }.ToList();
        var settings = await appSettingsService.GetMassAppSettingsAsync(keys);

        var nurseSetting = settings.FirstOrDefault(s => s.Key == AppSettingKeys.NurseIsRequiredForAppointment);
        var requireRoomSetting = settings.FirstOrDefault(s => s.Key == AppSettingKeys.AppointmentRequiresRoom);

        showNurses = nurseSetting?.GetBooleanValue() ?? false;
        showRooms = requireRoomSetting?.GetBooleanValue() ?? false;
    }

    public async Task HandleSaveClick()
    {
        validationComponent?.ClearErrors();

        // The form edits an AppointmentResponse; validate the request that will actually be sent.
        // Property names match, so the errors land on the right inputs.
        var validationResult = isEdit
            ? await UpdateValidator.ValidateAsync(appointmentDto.MapToUpdateDto())
            : await CreateValidator.ValidateAsync(appointmentDto.MapToCreationDto());

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
             Text = reason.ToDisplayString(),
         })
         .ToList();

        return appointmentReasons;
    }
}
