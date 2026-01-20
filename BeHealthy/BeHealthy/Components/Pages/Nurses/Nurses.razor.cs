using BeHealthy.Application.Dtos.Doctor;
using BeHealthy.Application.Dtos.Nurse;
using BeHealthy.Application.Extensions;
using BeHealthy.Application.Services;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Application.Validations.Nurse;
using BeHealthy.Common;
using BeHealthy.Components.Pages.Doctors;
using BeHealthy.Components.Shared.Modals;
using BeHealthy.Components.Shared.Wizards;
using BeHealthy.Domain;
using BeHealthy.Domain.Entities;
using BeHealthy.Models;
using BeHealthy.Models.Enums;
using BeHealthy.Shared.Locales;
using BeHealthy.Shared.Parameters;
using BeHealthy.States;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BeHealthy.Components.Pages.Nurses;

public partial class Nurses : BasePage
{
    [Inject] INurseService _nurseService { get; set; } = default!;

    [Inject] NavigationManager _navigationManager { get; set; } = default!;
    [Inject] AuthenticationStateProvider _authenticationStateProvider { get; set; } = default!;

    private List<NurseDto> _nurses { get; set; } = new();
    private QueryParameters QueryParameters { get; set; } = new();

    private string _selectedView = "Grid";
    private bool hasActionRights;
    private UserRole? _userRole;
    private string? _currentUserId;

    void ShowImportWizard()
    {
        ModalService.Show<MassImportWizard<NurseCreateDto>>(
            new Dictionary<string, object?>
            {
                { nameof(MassImportWizard<NurseCreateDto>.Entity), ImportEntity.Doctor },
                { nameof(MassImportWizard<NurseCreateDto>.OnSave), EventCallback.Factory.Create<(List<NurseCreateDto> nurseCreateDtos, bool UseValidation)>(this, BulkCreateNurses) },
            });
    }

    protected override void OnInitialized()
    {
        SetBreadcrumbs();
    }

    protected override async Task OnInitializedAsync()
    {
        LoaderService.SetLoader(true);

        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        _userRole = authState.User.GetUserRoleEnum();
        _currentUserId = authState.User.GetUserId();

        await LoadNurses(_currentUserId, _userRole);

        hasActionRights = _userRole == UserRole.Admin;

        LoaderService.SetLoader(false);
    }

    private void SetBreadcrumbs()
    {
        Breadcrumbs.SetBreadcrumbs(new List<Breadcrumb>()
        {
            new Breadcrumb(){ Text = Resource.Dashboard, Link = RoutingEndpoints.HOME_PAGE, Active = false },
            new Breadcrumb(){ Text = Resource.Nurses, Link = string.Empty, Active = true },
        });
    }

    private async Task LoadNurses(string? userId, UserRole? userRole = UserRole.Admin)
    {
        LoaderService.SetLoader(true);

        _nurses = userRole switch
        {
            UserRole.Patient when userId is not null => (await _nurseService.GetNursesOfPatientByUserId(userId, QueryParameters)).ToList(),
            _ => (await _nurseService.GetAllNursesAsync(QueryParameters)).ToList()
        };

        await InvokeAsync(StateHasChanged);
        LoaderService.SetLoader(false);
    }

    private void EditNurse(int id)
    {
        _navigationManager.NavigateTo($"{RoutingEndpoints.NURSES_PAGE}/edit/{id}");
    }

    private void CreateNurse()
    {
        _navigationManager.NavigateTo($"{RoutingEndpoints.NURSES_PAGE}/create");
    }

    private async Task BulkCreateNurses((List<NurseCreateDto> nurseForCreationDtos, bool UseValidation) result)
    {
        LoaderService.SetLoader(true);

        var useValidation = result.UseValidation;
        var validator = new NurseForCreationDtoValidator();

        foreach (var nurse in result.nurseForCreationDtos)
        {
            if (useValidation)
            {
                var validationResult = await validator.ValidateAsync(nurse);
                if (!validationResult.IsValid)
                {
                    AlertModalStateService.Show(null, validationResult.Errors.FirstOrDefault()?.ErrorMessage);
                    LoaderService.SetLoader(false);
                    return;
                }
            }

            var response = await _nurseService.AddNurseAsync(nurse);
            if (HandleServiceResponse(response)) continue;
        }
        await LoadNurses(_currentUserId, _userRole);
        LoaderService.SetLoader(false);
    }

    private async Task HandleSearch(string term)
    {
        QueryParameters.SearchTerm = term;
        await LoadNurses(_currentUserId, _userRole);
    }

    private async Task HandleClearFilters()
    {
        QueryParameters.SearchTerm = "";
        await LoadNurses(_currentUserId, _userRole);
    }

    private void ConfirmDelete(int nurseId)
    {
        ModalService.Show<ConfirmDeleteModal>(
           new Dictionary<string, object?>
           {
               { nameof(ConfirmDeleteModal.OnConfirm), () => OnConfirmDeleteAsync(nurseId) }
           });
    }

    private async Task OnConfirmDeleteAsync(int nurseId)
    {
        await _nurseService.DeleteNurseAsync(nurseId);
        await LoadNurses(_currentUserId, _userRole);
    }
}
