using BeHealthy.Client.Persistance;
using BeHealthy.Shared.Interfaces;
using BeHealthy.Shared.Models.Dtos.Nurse;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace BeHealthy.Client.Pages.Nurses;

public partial class Index
{
    private string _createUserHref { get; set; } = default!;
    [Inject] IUserService _userService { get; set; } = default!;
    [Inject] INurseService _nurseService { get; set; } = default!;
    [Inject] NavigationManager _navigationManager { get; set; } = default!;

    private List<NurseDto> _nurses { get; set; } = default!;

    private bool _isLoading = default;
    private string _selectedView = "Card";

    private PaginationState _paginationState = new();

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;
        _createUserHref = $"Account/Register?role=Nurse&redirectUrl={RoutingEndpoints.NURSES_PAGE}";
        _nurses = (await _nurseService.GetAllNursesAsync()).ToList();
        _paginationState.ItemsPerPage = 10;
        _isLoading = false;
    }

    private void OnPageSizeChanged(ChangeEventArgs e)
    {
        if (e.Value is not null)
        {
            _paginationState.ItemsPerPage = int.Parse((string)e.Value);
        }
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
