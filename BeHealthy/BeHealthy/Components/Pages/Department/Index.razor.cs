using BeHealthy.Shared.Interfaces;
using BeHealthy.Shared.Models.Dtos.Department;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace BeHealthy.Components.Pages.Department;

public partial class Index
{
    [Inject] IDepartmentService _departmentService { get; set; } = default!;
    [Inject] IPatientService _patientService { get; set; } = default!;
    [Inject] IDoctorService _doctorService { get; set; } = default!;
    [Inject] INurseService _nurseService { get; set; } = default!;

    private List<DepartmentDto> _departments = new();

    private bool _isLoading = false;
    private bool _hasActionRights;
    private PaginationState _paginationState = new();

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;
        _hasActionRights = true;
        _paginationState.ItemsPerPage = 10;
        _departments = (await _departmentService.GetAllDepartmentsAsync()).ToList();
        _isLoading = false;
    }

    private void OnPageSizeChanged(ChangeEventArgs e)
    {
        if (e.Value is not null)
        {
            _paginationState.ItemsPerPage = int.Parse((string)e.Value);
        }
    }

    public void EditDepartment(int departmentId)
    {

    }

    public void DeleteDepartment(int departmentId)
    {

    }
}
