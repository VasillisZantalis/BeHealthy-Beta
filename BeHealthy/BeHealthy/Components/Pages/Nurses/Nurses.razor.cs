using BeHealthy.Application.Dtos.Nurse;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Common;
using BeHealthy.Domain;
using BeHealthy.Models;
using BeHealthy.Shared.Locales;
using BeHealthy.States;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace BeHealthy.Components.Pages.Nurses;

public partial class Nurses : BasePage
{
    private string _createUserHref { get; set; } = default!;
    [Inject] INurseService _nurseService { get; set; } = default!;

    [Inject] NavigationManager _navigationManager { get; set; } = default!;

    private List<NurseDto> _nurses { get; set; } = default!;

    private string _selectedView = "Card";
    private bool hasActionRights;
    private bool hasEditRight;
    private bool hasDeleteRight;

    private PaginationState _paginationState = new();

    protected override async Task OnInitializedAsync()
    {
        LoaderService.SetLoader(true);

        SetBreadcrumbs();

        _nurses = (await _nurseService.GetAllNursesAsync()).ToList();
        _paginationState.ItemsPerPage = 10;
        //hasEditRight = await PrivilegeStateService.HasPrivilegeAsync(PrivilegeName.EditAppointments);
        //hasDeleteRight = await PrivilegeStateService.HasPrivilegeAsync(PrivilegeName.DeleteAppointments);
        hasActionRights = hasEditRight || hasDeleteRight;

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

    private void OnPageSizeChanged(ChangeEventArgs e)
    {
        if (e.Value is not null)
        {
            _paginationState.ItemsPerPage = int.Parse((string)e.Value);
        }
    }

    private void EditNurse(int id)
    {
        _navigationManager.NavigateTo($"{RoutingEndpoints.NURSES_PAGE}/edit/{id}");
    }

    private void CreateNurse()
    {
        _navigationManager.NavigateTo($"{RoutingEndpoints.NURSES_PAGE}/create");
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
