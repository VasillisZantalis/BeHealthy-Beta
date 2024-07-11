using BeHealthy.Client.Services.Interfaces;
using BeHealthy.Shared.Models.Entities;
using Microsoft.AspNetCore.Components;

namespace BeHealthy.Client.Pages.Nurses;

public partial class Index
{
    private string _createUserHref { get; set; } = default!;

    [Inject] IUserService _userService { get; set; } = default!;
    [Inject] NavigationManager _navigationManager { get; set; } = default!;

    private List<ApplicationUser> _nurses { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _createUserHref = "Account/Register?role=Nurse";
        _nurses = (await _userService.GetAllNursesAsync()).ToList();
    }

    private async Task EditNurse(string id)
    {
    }

    private async Task DeleteNurse(string id)
    {
        await _userService.DeleteUserAsync(id);
        _navigationManager.Refresh(forceReload: true);
    }
}
