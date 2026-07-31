using BeHealthy.Shared.Dtos.Doctor;
using BeHealthy.Frontend.Services.CurrentUser;
using BeHealthy.Frontend.Extensions;
using BeHealthy.Frontend.Services.Interfaces;
using BeHealthy.Frontend.Validations.Doctor;
using BeHealthy.Frontend.Common;
using BeHealthy.Frontend.Components.Shared.Modals;
using BeHealthy.Frontend.Components.Shared.Wizards;
using BeHealthy.Shared;
using BeHealthy.Frontend.Models;
using BeHealthy.Frontend.Models.Enums;
using BeHealthy.Shared.Locales;
using BeHealthy.Shared.Parameters;
using Microsoft.AspNetCore.Components;

namespace BeHealthy.Frontend.Components.Pages.Doctors;

public partial class Doctors : BasePage
{
    [Inject] IDoctorService DoctorService { get; set; } = default!;
    [Inject] IPatientService PatientsService { get; set; } = default!;
    [Inject] ISpecialtyService SpecialtyService { get; set; } = default!;
    [Inject] NavigationManager NavigationManager { get; set; } = default!;
    [Inject] ICurrentUserService CurrentUser { get; set; } = default!;

    private DoctorQueryParameters QueryParameters { get; set; } = new();
    private List<DoctorDto> doctors = new();
    private List<SelectItem> specialties = new();
    private HashSet<int> selectedDoctorIds = new();
    private int totalCount = 0;

    private string selectedView = "Grid";
    private bool hasActionRights;
    private UserRole? userRole;
    private string? currentUserId;

    protected override void OnInitialized()
    {
        SetBreadcrumbs();
    }

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        userRole = CurrentUser.Role;
        currentUserId = CurrentUser.UserId;

        await LoadDoctors();
        await LoadSpecialties();

        hasActionRights = userRole == UserRole.Admin;
        IsLoading = false;
    }

    private async Task LoadSpecialties()
    {
        var data = await SpecialtyService.GetSpecialtiesAsync();

        specialties = data.Select(s => new SelectItem
        {
            Value = s.Id,
            Text = s.Name,
        }).ToList();
        specialties.Insert(0, new SelectItem { Value = 0, Text = Resource.All });
    }

    private async Task HandleSearch(string term)
    {
        QueryParameters.SearchTerm = term;
        await LoadDoctors();
    }

    private async Task HandleSpecialtyFilter(int? specialtyId)
    {
        QueryParameters.SpecialtyId = specialtyId;
        await LoadDoctors();
    }

    private async Task HandlePageChanged(int page)
    {
        QueryParameters.PageNumber = page;
        await LoadDoctors();
    }

    private async Task HandlePageSizeChanged(int pageSize)
    {
        QueryParameters.PageSize = pageSize;
        QueryParameters.PageNumber = 1;
        await LoadDoctors();
    }

    private async Task HandleSortChanged((string? sortProperty, bool sortDescending) sortInfo)
    {
        QueryParameters.OrderBy = sortInfo.sortProperty;
        QueryParameters.OrderDescending = sortInfo.sortDescending;
        QueryParameters.PageNumber = 1;
        await LoadDoctors();
    }

    private async Task HandleClearFilters()
    {
        if (string.IsNullOrEmpty(QueryParameters.SearchTerm) && QueryParameters.SpecialtyId is null)
            return;

        QueryParameters.SearchTerm = "";
        QueryParameters.SpecialtyId = null;
        await LoadDoctors();
    }

    private void SetBreadcrumbs()
    {
        Breadcrumbs.SetBreadcrumbs(new List<Breadcrumb>()
        {
            new Breadcrumb(){ Text = Resource.Dashboard, Link = RoutingEndpoints.HOME_PAGE, Active = false },
            new Breadcrumb(){ Text = Resource.Doctors, Link = string.Empty, Active = true },
        });
    }

    private async Task LoadDoctors()
    {
        var role = userRole ?? UserRole.Admin;
        var userId = currentUserId;

        IsLoading = true;
        
        if (role == UserRole.Patient && userId is not null)
        {
            doctors = (await PatientsService.GetMyDoctorsAsync(userId)).ToList();
            totalCount = doctors.Count;
        }
        else
        {
            var paginatedResult = await DoctorService.GetAllDoctorsAsync(QueryParameters);
            doctors = paginatedResult.Items.ToList();
            totalCount = paginatedResult.TotalCount;
        }

        selectedDoctorIds.Clear();
        IsLoading = false;
        StateHasChanged();
    }

    private void ToggleDoctorSelection(int doctorId)
    {
        if (selectedDoctorIds.Contains(doctorId))
        {
            selectedDoctorIds.Remove(doctorId);
        }
        else
        {
            selectedDoctorIds.Add(doctorId);
        }
    }

    private void EditDoctor(int id)
    {
        NavigationManager.NavigateTo($"{RoutingEndpoints.DOCTORS_PAGE}/edit/{id}");
    }

    private void CreateDoctor()
    {
        NavigationManager.NavigateTo($"{RoutingEndpoints.DOCTORS_PAGE}/create");
    }

    private async Task BulkCreateDoctors((List<DoctorCreateDto> doctorCreateDtos, bool UseValidation) result)
    {
        IsLoading = true;
        var validator = result.UseValidation ? new DoctorCreateDtoValidator(false) : null;

        foreach (var doctor in result.doctorCreateDtos)
        {
            if (validator != null)
            {
                var validationResult = await validator.ValidateAsync(doctor);
                if (!validationResult.IsValid)
                {
                    AlertModalStateService.Show(null, validationResult.Errors.FirstOrDefault()?.ErrorMessage);
                    IsLoading = false;
                    return;
                }
            }

            var response = await DoctorService.AddDoctorAsync(doctor);
            if (!HandleServiceResponse(response))
            {
                IsLoading = false;
                return;
            }
        }
        
        await LoadDoctors();
        IsLoading = false;
    }

    void ShowImportWizard()
    {
        ModalService.Show<MassImportWizard<DoctorCreateDto>>(
            new Dictionary<string, object?>
            {
                { nameof(MassImportWizard<DoctorCreateDto>.Entity), ImportEntity.Doctor },
                { nameof(MassImportWizard<DoctorCreateDto>.OnSave), EventCallback.Factory.Create<(List<DoctorCreateDto> doctorCreateDtos, bool UseValidation)>(this, BulkCreateDoctors) },
            });
    }

    private void ConfirmDelete(int doctorId)
    {
        ConfirmDelete([doctorId]);
    }

    private void ConfirmBulkDelete()
    {
        ConfirmDelete(selectedDoctorIds.ToList());
    }

    private void ConfirmDelete(IEnumerable<int> doctorIds)
    {
        ModalService.Show<ConfirmDeleteModal>(
           new Dictionary<string, object?>
           {
               { nameof(ConfirmDeleteModal.OnConfirm), () => OnConfirmDeleteAsync(doctorIds) },
           });
    }

    private async Task OnConfirmDeleteAsync(IEnumerable<int> doctorIds)
    {
        IsLoading = true;
        foreach (var doctorId in doctorIds)
        {
            await DoctorService.DeleteDoctorAsync(doctorId);
        }

        await LoadDoctors();
        IsLoading = false;
    }
}
