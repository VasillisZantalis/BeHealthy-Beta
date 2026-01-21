using BeHealthy.Application.Dtos.Nurse;
using BeHealthy.Application.Extensions;
using BeHealthy.Application.Services;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Application.Validations.Nurse;
using BeHealthy.Common;
using BeHealthy.Components.Shared.Modals;
using BeHealthy.Components.Shared.Wizards;
using BeHealthy.Domain;
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
    [Inject] INurseService NurseService { get; set; } = default!;

    [Inject] NavigationManager NavigationManager { get; set; } = default!;
    [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

    private List<NurseDto> nurses { get; set; } = new();
    private QueryParameters QueryParameters { get; set; } = new();
    private HashSet<int> selectedNurseIds = new();

    private string selectedView = "Grid";
    private bool hasActionRights;
    private UserRole? userRole;
    private string? currentUserId;

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

        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        userRole = authState.User.GetUserRoleEnum();
        currentUserId = authState.User.GetUserId();

        await LoadNurses(currentUserId, userRole);

        hasActionRights = userRole == UserRole.Admin;

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

        nurses = userRole switch
        {
            UserRole.Patient when userId is not null => (await NurseService.GetNursesOfPatientByUserId(userId)).ToList(),
            _ => (await NurseService.GetAllNursesAsync(QueryParameters)).ToList()
        };

        selectedNurseIds.Clear();

        await InvokeAsync(StateHasChanged);
        LoaderService.SetLoader(false);
    }

    private void ToggleNurseSelection(int nurseId)
    {
        if (selectedNurseIds.Contains(nurseId))
        {
            selectedNurseIds.Remove(nurseId);
        }
        else
        {
            selectedNurseIds.Add(nurseId);
        }
    }

    private void EditNurse(int id)
    {
        NavigationManager.NavigateTo($"{RoutingEndpoints.NURSES_PAGE}/edit/{id}");
    }

    private void CreateNurse()
    {
        NavigationManager.NavigateTo($"{RoutingEndpoints.NURSES_PAGE}/create");
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

            var response = await NurseService.AddNurseAsync(nurse);
            if (HandleServiceResponse(response)) continue;
        }
        await LoadNurses(currentUserId, userRole);
        LoaderService.SetLoader(false);
    }

    private async Task HandleSearch(string term)
    {
        QueryParameters.SearchTerm = term;
        await LoadNurses(currentUserId, userRole);
    }

    private async Task HandleClearFilters()
    {
        if (string.IsNullOrEmpty(QueryParameters.SearchTerm)) return;

        QueryParameters.SearchTerm = "";
        await LoadNurses(currentUserId, userRole);
    }

    private void ConfirmDelete(int nurseId)
    {
        ConfirmDelete([nurseId]);
    }

    private void ConfirmBulkDelete()
    {
        ConfirmDelete(selectedNurseIds.ToList());
    }

    private void ConfirmDelete(IEnumerable<int> nurseIds)
    {
        var nurseIdsList = nurseIds.ToList();

        ModalService.Show<ConfirmDeleteModal>(
           new Dictionary<string, object?>
           {
               { nameof(ConfirmDeleteModal.OnConfirm), () => OnConfirmDeleteAsync(nurseIdsList) },
           });
    }

    private async Task OnConfirmDeleteAsync(IEnumerable<int> nurseIds)
    {
        LoaderService.SetLoader(true);

        foreach (var nurseId in nurseIds)
        {
            await NurseService.DeleteNurseAsync(nurseId);
        }

        selectedNurseIds.Clear();
        await LoadNurses(currentUserId, userRole);

        LoaderService.SetLoader(false);
    }
}
