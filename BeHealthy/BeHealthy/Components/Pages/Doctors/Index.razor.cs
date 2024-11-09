using BeHealthy.Extensions;
using BeHealthy.Persistance;
using BeHealthy.Services.Interfaces;
using BeHealthy.Shared.Models;
using BeHealthy.Shared.Models.Dtos.Doctor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.QuickGrid;
using System.Security.Claims;

namespace BeHealthy.Components.Pages.Doctors;

public partial class Index
{
    private string _createUserHref { get; set; } = default!;
    [Inject] IUserService _userService { get; set; } = default!;
    [Inject] IDoctorService _doctorService { get; set; } = default!;
    [Inject] IPrivilegeService _privilegeService { get; set; } = default!;
    [Inject] NavigationManager _navigationManager { get; set; } = default!;
    [Inject] AuthenticationStateProvider _authenticationStateProvider { get; set; } = default!;

    private List<DoctorDto> _doctors { get; set; } = default!;

    private bool _isLoading = default;
    private string _selectedView = "Card";
    private bool hasActionRights;
    private bool hasEditRight;
    private bool hasDeleteRight;

    private PaginationState _paginationState = new();

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();

        var userRole = Enum.Parse<UserRole>(authState.User.FindFirst(c => c.Type == ClaimTypes.Role)?.Value!);

        _createUserHref = $"{RoutingEndpoints.HOME_PAGE}/create";
        _doctors = (await _doctorService.GetAllDoctorsAsync()).ToList();
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

    private void EditDoctor(int id)
    {
        _navigationManager.NavigateTo($"{RoutingEndpoints.DOCTORS_PAGE}/edit/{id}");
    }

    private void CreateDoctor()
    {
        _navigationManager.NavigateTo($"{RoutingEndpoints.DOCTORS_PAGE}/create");
    }

    private async Task DeleteDoctor(int id)
    {
        await _doctorService.DeleteDoctorAsync(id);
        _navigationManager.Refresh(forceReload: true);
    }
}
