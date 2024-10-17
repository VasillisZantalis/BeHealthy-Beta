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

    private bool _isLoading = default;

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;
        _createUserHref = $"Account/Register?role=Nurse&redirectUrl={RoutingEndpoints.NURSES_PAGE}";
        _nurses = (await _nurseService.GetAllNursesAsync()).ToList();
        _isLoading = false;
    }

    private async Task EditNurse(int id)
    {
    }

    private async Task DeleteNurse(int id)
    {
        await _nurseService.DeleteNurseAsync(id);
        _navigationManager.Refresh(forceReload: true);
    }
}
