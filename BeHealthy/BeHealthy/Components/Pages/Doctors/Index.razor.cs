using BeHealthy.Persistance;
using BeHealthy.Shared.Interfaces;
using BeHealthy.Shared.Models.Dtos.Doctor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace BeHealthy.Components.Pages.Doctors;

public partial class Index
{
    private string _createUserHref { get; set; } = default!;
    [Inject] IUserService _userService { get; set; } = default!;
    [Inject] IDoctorService _doctorService { get; set; } = default!;
    [Inject] NavigationManager _navigationManager { get; set; } = default!;

    private List<DoctorDto> _doctors { get; set; } = default!;

    private bool _isLoading = default;
    private string _selectedView = "Card";

    private PaginationState _paginationState = new();

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;
        _createUserHref = $"Account/Register?role=Doctor&redirectUrl={RoutingEndpoints.HOME_PAGE}";
        _doctors = (await _doctorService.GetAllDoctorsAsync()).ToList();
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

    private async Task EditDoctor(int id)
    {
        _navigationManager.NavigateTo($"{RoutingEndpoints.DOCTORS_PAGE}/{id}");
    }

    private async Task DeleteDoctor(int id)
    {
        await _doctorService.DeleteDoctorAsync(id);
        _navigationManager.Refresh(forceReload: true);
    }
}
