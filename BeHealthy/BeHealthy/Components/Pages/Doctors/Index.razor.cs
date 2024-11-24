using BeHealthy.Application.Dtos.Doctor;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Components.Shared.Modals;
using BeHealthy.Persistance;
using BeHealthy.States;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace BeHealthy.Components.Pages.Doctors;

public partial class Index : BasePage
{
    [Inject] IDoctorService _doctorService { get; set; } = default!;
    [Inject] NavigationManager _navigationManager { get; set; } = default!;

    private List<DoctorDto> _doctors { get; set; } = default!;
    private ConfirmDeleteModal _confirmDeleteModal = new();

    private string _selectedView = "Card";
    private bool hasActionRights;
    private bool hasEditRight;
    private bool hasDeleteRight;

    private int deleteItemId;

    private PaginationState _paginationState = new();

    protected override async Task OnInitializedAsync()
    {
        LoaderService.SetLoader(true);

        _doctors = (await _doctorService.GetAllDoctorsAsync()).ToList();
        _paginationState.ItemsPerPage = 10;
        hasEditRight = await PrivilegeStateService.HasPrivilegeAsync("CanEditAppointment");
        hasDeleteRight = await PrivilegeStateService.HasPrivilegeAsync("CanDeleteAppointment");
        hasActionRights = hasEditRight || hasDeleteRight;

        LoaderService.SetLoader(false);
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

    private void ConfirmDelete(int id)
    {
        deleteItemId = id;
        _confirmDeleteModal.HandleOpen();
    }

    private async Task OnDeleteConfirmed(bool confirmed)
    {
        if (confirmed)
        {
            await _doctorService.DeleteDoctorAsync(deleteItemId);
            _navigationManager.Refresh(forceReload: true);
        }
    }
}
