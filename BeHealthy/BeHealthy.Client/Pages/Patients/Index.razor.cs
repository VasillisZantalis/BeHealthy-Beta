using BeHealthy.Client.Services.Interfaces;
using BeHealthy.Shared.Models.Entities;
using Microsoft.AspNetCore.Components;

namespace BeHealthy.Client.Pages.Patients;

public partial class Index
{
    private string _createUserHref { get; set; } = default!;

    [Inject] IUserService _userService { get; set; } = default!;
    [Inject] NavigationManager _navigationManager { get; set; } = default!;

    private List<ApplicationUser> _patients { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _createUserHref = "Account/Register?role=Patient";
        _patients = (await _userService.GetAllPatientsAsync()).ToList();
    }

    private async Task EditPatient(string id)
    {
    }

    private async Task DeletePatient(string id)
    {
        await _userService.DeleteUserAsync(id);
        _navigationManager.Refresh(forceReload: true);
    }
}
