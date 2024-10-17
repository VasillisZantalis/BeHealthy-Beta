using BeHealthy.Client.Persistance;
using BeHealthy.Shared.Interfaces;
using BeHealthy.Shared.Models.Dtos.Doctor;
using Microsoft.AspNetCore.Components;

namespace BeHealthy.Client.Pages.Doctors;

public partial class Index
{
    private string _createUserHref { get; set; } = default!;
    [Inject] IUserService _userService { get; set; } = default!;
    [Inject] IDoctorService _doctorService { get; set; } = default!;
    [Inject] NavigationManager _navigationManager { get; set; } = default!;

    private List<DoctorDto> _doctors { get; set; } = default!;

    private bool _isLoading = default;

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;
        _createUserHref = $"Account/Register?role=Doctor&redirectUrl={RoutingEndpoints.HOME_PAGE}";
        _doctors = (await _doctorService.GetAllDoctorsAsync()).ToList();
        _isLoading = false;
    }

    private async Task EditDoctor(string id)
    {
    }

    private async Task DeleteDoctor(string id)
    {
        await _userService.DeleteUserAsync(id);
        _navigationManager.Refresh(forceReload: true);
    }
}
