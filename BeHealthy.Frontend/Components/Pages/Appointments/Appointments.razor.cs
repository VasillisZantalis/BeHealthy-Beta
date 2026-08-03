using BeHealthy.Shared.Dtos.Appointment;
using BeHealthy.Frontend.Mappings;
using BeHealthy.Frontend.Services.CurrentUser;
using BeHealthy.Shared.Dtos.Doctor;
using BeHealthy.Shared.Dtos.Patient;
using BeHealthy.Frontend.Extensions;
using BeHealthy.Frontend.Services.Interfaces;
using BeHealthy.Frontend.Common;
using BeHealthy.Frontend.Components.Shared.Modals;
using BeHealthy.Frontend.Components.Shared.Wizards;
using BeHealthy.Shared;
using BeHealthy.Frontend.Models;
using BeHealthy.Frontend.Models.Enums;
using BeHealthy.Shared.Locales;
using BeHealthy.Shared.Parameters;
using Microsoft.AspNetCore.Components;

namespace BeHealthy.Frontend.Components.Pages.Appointments;

public partial class Appointments : BasePage
{
    private List<AppointmentResponse> appointments = new();

    [Inject] IAppointmentService AppointmentService { get; set; } = default!;
    [Inject] IDoctorService DoctorService { get; set; } = default!;
    [Inject] INurseService NurseService { get; set; } = default!;
    [Inject] IPatientService PatientService { get; set; } = default!;
    [Inject] ICurrentUserService CurrentUser { get; set; } = default!;

    private List<DoctorSimpleResponse>? doctors { get; set; }
    private List<PatientSimpleResponse>? patients { get; set; }
    private List<SelectItem> doctorsSelect { get; set; } = new();
    private List<SelectItem> patientsSelect { get; set; } = new();
    private HashSet<int> selectedAppointmentIds = new();
    private AppointmentQueryParameters QueryParameters { get; set; } = new();

    private string? currentUserId;
    private bool hasActionRights;
    private bool hasEditRight;
    private bool hasDeleteRight;
    private int totalCount = 0;
    private UserRole? userRole;

    protected override void OnInitialized()
    {
        SetBreadcrumbs();
    }

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;

        currentUserId = CurrentUser.UserId;
        userRole = CurrentUser.Role;

        await Task.WhenAll(
            LoadAppointments(),
            LoadDoctors(),
            LoadPatients(),
            GetUserPrivilege(userRole!.Value)
        );

        hasActionRights = hasEditRight || hasDeleteRight;

        IsLoading = false;
    }

    private async Task LoadAppointments()
    {
        IsLoading = true;
        StateHasChanged();

        if (userRole == UserRole.Admin)
        {
            var paginatedResult = await AppointmentService.GetAllAppointmentsAsync(QueryParameters);
            appointments = paginatedResult.Items.ToList();
            totalCount = paginatedResult.TotalCount;
        }
        else
        {
            appointments = (userRole, currentUserId) switch
            {
                (UserRole.Doctor, not null) => (await DoctorService.GetDoctorAppointmentsByUserIdAsync(currentUserId)).ToList(),
                (UserRole.Patient, not null) => (await PatientService.GetPatientAppointmentsByUserIdAsync(currentUserId)).ToList(),
                _ => []
            };
        }


        selectedAppointmentIds.Clear();
        IsLoading = false;

        StateHasChanged();
    }

    private void SetBreadcrumbs()
    {
        Breadcrumbs.SetBreadcrumbs(new List<Breadcrumb>()
        {
            new Breadcrumb(){ Text = Resource.Dashboard, Link = RoutingEndpoints.HOME_PAGE, Active = false },
            new Breadcrumb(){ Text = Resource.Appointments, Link = string.Empty, Active = true },
        });
    }

    private async Task HandleAppointmentFormSubmission((AppointmentResponse, int?) submission)
    {
        var (appointmentDto, appointmentId) = submission;

        if (appointmentId is not null)
        {
            appointmentDto.Id = appointmentId.Value;
            var appointmentForUpdate = appointmentDto.MapToUpdateDto();
            await UpdateAppointmentAsync(appointmentForUpdate);
        }
        else
        {
            var appointmentForCreate = appointmentDto.MapToCreationDto();
            await CreateAppointmentAsync(appointmentForCreate);
        }
    }

    private async Task LoadDoctors()
    {
        doctors = (await DoctorService.GetAllDoctorsSimpleAsync()).ToList();

        if (userRole == UserRole.Doctor)
        {
            doctors = doctors.Where(d => d.UserId.ToString() == currentUserId).ToList();
        }

        doctorsSelect = doctors.Select(s => new SelectItem
        { 
            Text = s.FullName,
            Value = s.Id 
        }).ToList();
        doctorsSelect.Insert(0, new(){ Text = Resource.All, Value = 0 });
    }

    private async Task LoadPatients()
    {
        patients = (await PatientService.GetAllPatientsSimpleAsync()).ToList();
        patientsSelect = patients.Select(s => new SelectItem
        {
            Text = s.FullName,
            Value = s.Id
        }).ToList();
        patientsSelect.Insert(0, new() { Text = Resource.All, Value = 0 });
    }

    private void ToggleAppointmentSelection(int appointmentId)
    {
        if (selectedAppointmentIds.Contains(appointmentId))
        {
            selectedAppointmentIds.Remove(appointmentId);
        }
        else
        {
            selectedAppointmentIds.Add(appointmentId);
        }
    }

    private async Task HandleDoctorFilter(int? doctorId)
    {
        QueryParameters.DoctorId = doctorId;
        await LoadAppointments();
    }

    private async Task HandlePatientFilter(int? patientId)
    {
        QueryParameters.PatientId = patientId;
        await LoadAppointments();
    }

    private async Task HandlePageChanged(int page)
    {
        QueryParameters.PageNumber = page;
        await LoadAppointments();
    }

    private async Task HandlePageSizeChanged(int pageSize)
    {
        QueryParameters.PageSize = pageSize;
        QueryParameters.PageNumber = 1;
        await LoadAppointments();
    }

    private async Task HandleSortChanged((string? sortProperty, bool sortDescending) sortInfo)
    {
        QueryParameters.OrderBy = sortInfo.sortProperty;
        QueryParameters.OrderDescending = sortInfo.sortDescending;
        QueryParameters.PageNumber = 1;
        await LoadAppointments();
    }


    private void CreateAppointment()
    {
        ShowAppointmentModal(null);
    }

    private void EditAppointment(int appointmentId)
    {
        ShowAppointmentModal(appointmentId);
    }

    private void ShowAppointmentModal(int? appointmentId)
    {
        ModalService.Show<AppointmentModal>(
            new Dictionary<string, object?>
            {
                { nameof(AppointmentModal.AppointmentId), appointmentId },
                { nameof(AppointmentModal.OnFormSubmit), EventCallback.Factory.Create<(AppointmentResponse, int?)>(this, HandleAppointmentFormSubmission) },
                { nameof(AppointmentModal.Doctors), doctors! },
                { nameof(AppointmentModal.Patients), patients! },
                { nameof(AppointmentModal.Role), userRole },
                { nameof(AppointmentModal.CurrentUserId), currentUserId! }
            });
    }

    private void ConfirmDelete(int appointmentId)
    {
        ConfirmDelete([appointmentId]);
    }

    private void ConfirmBulkDelete()
    {
        ConfirmDelete(selectedAppointmentIds.ToList());
    }

    private void ConfirmDelete(IEnumerable<int> appointmentIds)
    {
        var appointmentIdsList = appointmentIds.ToList();

        ModalService.Show<ConfirmDeleteModal>(
           new Dictionary<string, object?>
           {
               { nameof(ConfirmDeleteModal.OnConfirm), () => OnConfirmDeleteAsync(appointmentIdsList) }
           });
    }

    private async Task OnConfirmDeleteAsync(IEnumerable<int> appointmentIds)
    {
        IsLoading = true;

        foreach (var appointmentId in appointmentIds)
        {
            await AppointmentService.DeleteAppointmentAsync(appointmentId);
        }

        IsLoading = false;
        await LoadAppointments();
    }

    private async Task GetUserPrivilege(UserRole userRole)
    {
        (hasEditRight, hasDeleteRight) = userRole switch
        {
            UserRole.Admin => (true, true),
            _ => (await GetPrivilege(userRole, PrivilegeName.EditAppointments),
                await GetPrivilege(userRole, PrivilegeName.DeleteAppointments))
        };
    }

    private async Task<bool> GetPrivilege(UserRole role, PrivilegeName privilege)
    {
        return false;
        //return await privilegeService.HasPrivilegeAsync(role, privilege);
    }

    private async Task CreateAppointmentAsync(AppointmentCreateRequest AppointmentCreateRequest, bool fromBulkCreation = false)
    {
        var response = await AppointmentService.AddAppointmentAsync(AppointmentCreateRequest);
        if (HandleServiceResponse(response))
        {
            if (!fromBulkCreation)
                await LoadAppointments();
        }
    }

    private async Task UpdateAppointmentAsync(AppointmentUpdateRequest appointmentForUpdateDto)
    {
        var response = await AppointmentService.UpdateAppointmentAsync(appointmentForUpdateDto);

        if (HandleServiceResponse(response))
            await LoadAppointments();
    }

    private async Task BulkCreateAppointments((List<AppointmentCreateRequest> AppointmentCreateDtos, bool UseValidation) result)
    {
        IsLoading = true;
        foreach (var appointment in result.AppointmentCreateDtos)
        {
            await CreateAppointmentAsync(appointment, true);
        }

        IsLoading = false;
        await LoadAppointments();
    }

    void ShowImportWizard()
    {
        ModalService.Show<MassImportWizard<AppointmentCreateRequest>>(
            new Dictionary<string, object?>
            {
                { nameof(MassImportWizard<DoctorCreateRequest>.Entity), ImportEntity.Appointment },
                { nameof(MassImportWizard<DoctorCreateRequest>.OnSave), EventCallback.Factory.Create<(List<AppointmentCreateRequest> appointmentCreateDtos, bool UseValidation)>(this, BulkCreateAppointments) },
            });
    }
}
