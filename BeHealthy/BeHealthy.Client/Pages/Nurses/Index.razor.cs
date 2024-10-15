using BeHealthy.Client.Persistance;
using BeHealthy.Shared.Interfaces;
using BeHealthy.Shared.Models.Dtos.Nurse;
using Microsoft.AspNetCore.Components;

namespace BeHealthy.Client.Pages.Nurses;

public partial class Index
{
    private string _createUserHref { get; set; } = default!;
    [Inject] IUserService _userService { get; set; } = default!;
    [Inject] INurseService _nurseService { get; set; } = default!;
    [Inject] NavigationManager _navigationManager { get; set; } = default!;

    private List<NurseDto> _nurses { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _createUserHref = $"Account/Register?role=Nurse&redirectUrl={RoutingEndpoints.NursesPage}";
        _nurses = (await _nurseService.GetAllNursesAsync()).ToList();
    }

    //private async Task EditNurse(string id)
    //{
    //}

    private async Task DeleteNurse(string id)
    {
        await _userService.DeleteUserAsync(id);
        _navigationManager.Refresh(forceReload: true);
    }
}
