using BeHealthy.Application.Dtos.Nurse;
using BeHealthy.Application.Extensions;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Components.Shared.Modals;
using BeHealthy.Domain;
using BeHealthy.Persistance;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace BeHealthy.Components.Pages.Nurses;

public partial class Index
{
    private string _createUserHref { get; set; } = default!;
    [Inject] IUserService _userService { get; set; } = default!;
    [Inject] INurseService _nurseService { get; set; } = default!;
    [Inject] IPrivilegeService _privilegeService { get; set; } = default!;
    [Inject] NavigationManager _navigationManager { get; set; } = default!;
    [Inject] AuthenticationStateProvider _authenticationStateProvider { get; set; } = default!;

    private List<NurseDto> _nurses { get; set; } = default!;
    private ConfirmDeleteModal _confirmDeleteModal = new();

    private bool _isLoading = default;
    private string _selectedView = "Card";
    private bool hasActionRights;
    private bool hasEditRight;
    private bool hasDeleteRight;

    private int deleteItemId;

    private PaginationState _paginationState = new();

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var userRole = Enum.Parse<UserRole>(authState.User.GetUserRole());

        _nurses = (await _nurseService.GetAllNursesAsync()).ToList();
        _paginationState.ItemsPerPage = 10;
        hasEditRight = await _privilegeService.HasPrivilege(userRole, "CanEditAppointment");
        hasDeleteRight = await _privilegeService.HasPrivilege(userRole, "CanDeleteAppointment");
        hasActionRights = hasEditRight || hasDeleteRight;
        _isLoading = false;
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

    private void ConfirmDelete(int id)
    {
        deleteItemId = id;
        _confirmDeleteModal.HandleOpen();
    }

    private async Task OnDeleteConfirmed(bool confirmed)
    {
        if (confirmed)
        {
            await _nurseService.DeleteNurseAsync(deleteItemId);
            _navigationManager.Refresh(forceReload: true);
        }
    }
}
