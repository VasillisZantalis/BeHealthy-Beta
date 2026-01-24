using BeHealthy.Application.Dtos.Appointment;
using BeHealthy.Application.Dtos.Doctor;
using BeHealthy.Application.Dtos.Patient;
using BeHealthy.Application.Extensions;
using BeHealthy.Application.Mappings;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Common;
using BeHealthy.Components.Shared.Modals;
using BeHealthy.Domain;
using BeHealthy.Models;
using BeHealthy.Services.Interfaces;
using BeHealthy.Shared.Locales;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.QuickGrid;
using System.Data;

namespace BeHealthy.Components.Pages.Appointments;

public partial class Appointments : BasePage
{
    private List<AppointmentDto> appointments = default!;

    [Inject] IAppointmentService AppointmentService { get; set; } = default!;
    [Inject] IDoctorService DoctorService { get; set; } = default!;
    [Inject] INurseService NurseService { get; set; } = default!;
    [Inject] IPatientService PatientService { get; set; } = default!;
    [Inject] NavigationManager NavigationManager { get; set; } = default!;
    [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

    private List<DoctorSimpleDto>? doctors { get; set; }
    private List<PatientSimpleDto>? patients { get; set; }

    private AppointmentModal appointmentModal { get; set; } = new();

    private string? currentUserId;
    private bool hasActionRights;
    private bool hasEditRight;
    private bool hasDeleteRight;
    private int? appointmentId;

    private UserRole? userRole;

    private PaginationState paginationState = new();

    private bool showWizard = false;
    void ShowImportWizard() => showWizard = true;
    void HideImportWizard() => showWizard = false;

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

        await LoadAppointments();

        await GetUserPrivilege(userRole!.Value);

        hasActionRights = hasEditRight || hasDeleteRight;

        await LoadDoctors();
        await LoadPatients();

        paginationState.ItemsPerPage = appointments.Count;

        LoaderService.SetLoader(false);
    }

    private async Task LoadAppointments()
    {
        switch (userRole, currentUserId)
        {
            case (UserRole.Doctor, not null):
                appointments = (await DoctorService.GetDoctorAppointmentsByUserIdAsync(currentUserId)).ToList();
                break;

            case (UserRole.Patient, not null):
                appointments = (await PatientService.GetPatientAppointmentsByUserIdAsync(currentUserId)).ToList();
                break;

            default:
                appointments = (await AppointmentService.GetAllAppointmentsAsync()).ToList();
                break;
        }
        paginationState.ItemsPerPage = appointments.Count;
        await InvokeAsync(StateHasChanged);
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
        appointmentModal.Close();

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

    private void EditAppointment(int appointmentId)
    {
        this.appointmentId = appointmentId;
        appointmentModal.Open();
    }

    private void ConfirmDelete(int appointmentId)
    {
        ModalService.Show<ConfirmDeleteModal>(
           new Dictionary<string, object?>
           {
               { nameof(ConfirmDeleteModal.OnConfirm), () => OnConfirmDeleteAsync(appointmentId) }
           });
    }

    private async Task OnConfirmDeleteAsync(int appointmentId)
    {
        await AppointmentService.DeleteAppointmentAsync(appointmentId);
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
                NavigationManager.Refresh(true);
        }
    }

    private async Task UpdateAppointmentAsync(AppointmentUpdateDto appointmentForUpdateDto)
    {
        var response = await AppointmentService.UpdateAppointmentAsync(appointmentForUpdateDto);

        if (HandleServiceResponse(response))
            NavigationManager.Refresh(true);
    }

    private async Task BulkCreateAppointments((List<AppointmentCreateDto> AppointmentCreateDtos, bool UseValidation) result)
    {
        var useValidation = result.UseValidation;
        foreach (var appointment in result.AppointmentCreateDtos)
        {
            await CreateAppointmentAsync(appointment, true);
        }
        await LoadAppointments();
    }
}
