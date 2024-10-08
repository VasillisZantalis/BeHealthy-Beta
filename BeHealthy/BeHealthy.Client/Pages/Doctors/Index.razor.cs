using BeHealthy.Client.Services.Interfaces;
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

    protected override async Task OnInitializedAsync()
    {
        _createUserHref = "Account/Register?role=Doctor";
        //_doctors = (await _userService.GetAllDoctorsAsync()).ToList();
        _doctors = (await _doctorService.GetAllDoctorsAsync()).ToList();
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
