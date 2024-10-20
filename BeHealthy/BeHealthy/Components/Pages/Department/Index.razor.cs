using BeHealthy.Shared.Interfaces;
using BeHealthy.Shared.Models.Dtos.Department;
using Microsoft.AspNetCore.Components;

namespace BeHealthy.Components.Pages.Department;

public partial class Index
{
    [Inject] IDepartmentService _departmentService { get; set; } = default!;
    [Inject] IPatientService _patientService { get; set; } = default!;
    [Inject] IDoctorService _doctorService { get; set; } = default!;
    [Inject] INurseService _nurseService { get; set; } = default!;

    private List<DepartmentDto> _departments = new();

    private bool _isLoading = false;

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;
        _departments = (await _departmentService.GetAllDepartmentsAsync()).ToList();
        _isLoading = false;
    }
}
