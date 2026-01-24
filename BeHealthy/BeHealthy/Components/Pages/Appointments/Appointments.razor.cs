using BeHealthy.Application.Dtos.Appointment;
using BeHealthy.Application.Dtos.Doctor;
using BeHealthy.Application.Dtos.Patient;
using BeHealthy.Application.Extensions;
using BeHealthy.Application.Mappings;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Common;
using BeHealthy.Components.Shared.Modals;
using BeHealthy.Components.Shared.Wizards;
using BeHealthy.Domain;
using BeHealthy.Models;
using BeHealthy.Models.Enums;
using BeHealthy.Services.Interfaces;
using BeHealthy.Shared.Locales;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BeHealthy.Components.Pages.Appointments;

public partial class Appointments : BasePage
{
    private List<AppointmentDto> appointments = new();

    [Inject] IAppointmentService AppointmentService { get; set; } = default!;
    [Inject] IDoctorService DoctorService { get; set; } = default!;
    [Inject] INurseService NurseService { get; set; } = default!;
    [Inject] IPatientService PatientService { get; set; } = default!;
    [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

    private List<DoctorSimpleDto>? doctors { get; set; }
    private List<PatientSimpleDto>? patients { get; set; }
    private HashSet<int> selectedAppointmentIds = new();

    private string? currentUserId;
    private bool hasActionRights;
    private bool hasEditRight;
    private bool hasDeleteRight;

    private UserRole? userRole;

    protected override void OnInitialized()
    {
        SetBreadcrumbs();
    }

    protected override async Task OnInitializedAsync()
    {
        LoaderService.SetLoader(true);

        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        currentUserId = authState.User.GetUserId();
        userRole = authState.User.GetUserRoleEnum();

        await Task.WhenAll(
            LoadAppointments(),
            LoadDoctors(),
            LoadPatients(),
            GetUserPrivilege(userRole!.Value)
        );

        hasActionRights = hasEditRight || hasDeleteRight;

        LoaderService.SetLoader(false);
    }

    private async Task LoadAppointments()
    {
        LoaderService.SetLoader(true);
        StateHasChanged();

        appointments = (userRole, currentUserId) switch
        {
            (UserRole.Doctor, not null) => (await DoctorService.GetDoctorAppointmentsByUserIdAsync(currentUserId)).ToList(),
            (UserRole.Patient, not null) => (await PatientService.GetPatientAppointmentsByUserIdAsync(currentUserId)).ToList(),
            _ => (await AppointmentService.GetAllAppointmentsAsync()).ToList()
        };

        selectedAppointmentIds.Clear();
        StateHasChanged();

        LoaderService.SetLoader(false);
    }

    private void SetBreadcrumbs()
    {
        Breadcrumbs.SetBreadcrumbs(new List<Breadcrumb>()
        {
            new Breadcrumb(){ Text = Resource.Dashboard, Link = RoutingEndpoints.HOME_PAGE, Active = false },
            new Breadcrumb(){ Text = Resource.Appointments, Link = string.Empty, Active = true },
        });
    }

    private async Task HandleAppointmentFormSubmission((AppointmentDto, int?) submission)
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
    }

    private async Task LoadPatients()
    {
        patients = (await PatientService.GetAllPatientsSimpleAsync()).ToList();
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
                { nameof(AppointmentModal.OnFormSubmit), EventCallback.Factory.Create<(AppointmentDto, int?)>(this, HandleAppointmentFormSubmission) },
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
        LoaderService.SetLoader(true);

        foreach (var appointmentId in appointmentIds)
        {
            await AppointmentService.DeleteAppointmentAsync(appointmentId);
        }

        LoaderService.SetLoader(false);
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

    private async Task CreateAppointmentAsync(AppointmentCreateDto AppointmentCreateDto, bool fromBulkCreation = false)
    {
        var response = await AppointmentService.AddAppointmentAsync(AppointmentCreateDto);
        if (HandleServiceResponse(response))
        {
            if (!fromBulkCreation)
                await LoadAppointments();
        }
    }

    private async Task UpdateAppointmentAsync(AppointmentUpdateDto appointmentForUpdateDto)
    {
        var response = await AppointmentService.UpdateAppointmentAsync(appointmentForUpdateDto);

        if (HandleServiceResponse(response))
            await LoadAppointments();
    }

    private async Task BulkCreateAppointments((List<AppointmentCreateDto> AppointmentCreateDtos, bool UseValidation) result)
    {
        LoaderService.SetLoader(true);

        foreach (var appointment in result.AppointmentCreateDtos)
        {
            await CreateAppointmentAsync(appointment, true);
        }

        await LoadAppointments();
        LoaderService.SetLoader(false);
    }

    private async Task HandleSearch(string term)
    {
        // Implement search logic if needed
        await Task.CompletedTask;
    }

    private async Task HandleClearFilters()
    {
        // Implement clear filters logic if needed
        await Task.CompletedTask;
    }

    void ShowImportWizard()
    {
        ModalService.Show<MassImportWizard<AppointmentCreateDto>>(
            new Dictionary<string, object?>
            {
                { nameof(MassImportWizard<DoctorCreateDto>.Entity), ImportEntity.Appointment },
                { nameof(MassImportWizard<DoctorCreateDto>.OnSave), EventCallback.Factory.Create<(List<AppointmentCreateDto> appointmentCreateDtos, bool UseValidation)>(this, BulkCreateAppointments) },
            });
    }
}
