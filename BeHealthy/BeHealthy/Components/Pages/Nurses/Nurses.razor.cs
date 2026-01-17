using BeHealthy.Application.Dtos.Doctor;
using BeHealthy.Application.Dtos.Nurse;
using BeHealthy.Application.Extensions;
using BeHealthy.Application.Services;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Application.Validations.Nurse;
using BeHealthy.Common;
using BeHealthy.Domain;
using BeHealthy.Models;
using BeHealthy.Shared.Locales;
using BeHealthy.States;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace BeHealthy.Components.Pages.Nurses;

public partial class Nurses : BasePage
{
    private string _createUserHref { get; set; } = default!;
    [Inject] INurseService _nurseService { get; set; } = default!;

    [Inject] NavigationManager _navigationManager { get; set; } = default!;
    [Inject] AuthenticationStateProvider _authenticationStateProvider { get; set; } = default!;

    private List<NurseDto> _nurses { get; set; } = default!;

    private string _selectedView = "Card";
    private bool hasActionRights;
    private UserRole? _userRole;
    private string? _currentUserId;

    private PaginationState _paginationState = new();

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

        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        _userRole = authState.User.GetUserRoleEnum();
        _currentUserId = authState.User.GetUserId();

        await LoadNurses(_currentUserId, _userRole);

        _paginationState.ItemsPerPage = _nurses.Count;
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
            UserRole.Patient when userId is not null => (await _nurseService.GetNursesOfPatientByUserId(userId)).ToList(),
            _ => (await _nurseService.GetAllNursesAsync()).ToList()
        };


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

    private void ConfirmDelete(int nurseId)
    {
        ConfirmDeleteService.RequestDelete(async () =>
        {
            await _nurseService.DeleteNurseAsync(nurseId);
            _navigationManager.Refresh(forceReload: true);
        });
    }
}
