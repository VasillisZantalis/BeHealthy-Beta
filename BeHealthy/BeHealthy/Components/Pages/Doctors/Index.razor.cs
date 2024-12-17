using BeHealthy.Application.Dtos.Doctor;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Domain;
using BeHealthy.Domain.Entities;
using BeHealthy.Models;
using BeHealthy.Persistance;
using BeHealthy.Shared.Locales;
using BeHealthy.States;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace BeHealthy.Components.Pages.Doctors;

public partial class Index : BasePage
{
    [Inject] IDoctorService _doctorService { get; set; } = default!;
    [Inject] NavigationManager _navigationManager { get; set; } = default!;

    private List<DoctorDto> _doctors { get; set; } = default!;

    private string _selectedView = "Card";
    private bool hasActionRights;
    private bool hasEditRight;
    private bool hasDeleteRight;

    private int deleteItemId;

    private PaginationState _paginationState = new();

    protected override async Task OnInitializedAsync()
    {
        LoaderService.SetLoader(true);

        SetBreadcrumbs();

        _doctors = (await _doctorService.GetAllDoctorsAsync()).ToList();
        _paginationState.ItemsPerPage = 10;
        hasEditRight = await PrivilegeStateService.HasPrivilegeAsync(PrivilegeName.EditAppointments);
        hasDeleteRight = await PrivilegeStateService.HasPrivilegeAsync(PrivilegeName.DeleteAppointments);
        hasActionRights = hasEditRight || hasDeleteRight;

        LoaderService.SetLoader(false);
    }

    private void SetBreadcrumbs()
    {
        Breadcrumbs.SetBreadcrumbs(new List<Breadcrumb>()
        {
            new Breadcrumb(){ Text = Resource.Dashboard, Link = RoutingEndpoints.HOME_PAGE, Active = false },
            new Breadcrumb(){ Text = Resource.Doctors, Link = string.Empty, Active = true },
        });
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

    private void ConfirmDelete(int doctorId)
    {
        ConfirmDeleteService.RequestDelete(async () => {
            await _doctorService.DeleteDoctorAsync(doctorId);
            _navigationManager.Refresh(forceReload: true);
        });
    }
}
