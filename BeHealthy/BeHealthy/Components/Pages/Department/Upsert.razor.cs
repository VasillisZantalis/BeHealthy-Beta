using BeHealthy.Shared.Interfaces;
using BeHealthy.Shared.Models.Dtos.Department;
using BeHealthy.Shared.Models.Dtos.Doctor;
using Microsoft.AspNetCore.Components;

namespace BeHealthy.Components.Pages.Department;

public partial class Upsert
{
    [Parameter]
    public int? id { get; set; }
    private bool IsEditMode => id.HasValue;
    private int activeTab = 0;
    private void SelectTab(int tabName) => activeTab = tabName;

    private DepartmentDto DepartmentDto = new ();
    private List<DoctorDto> Doctors = new();

    [Inject]
    private IDepartmentService _departmentService { get; set; } = default!;

    [Inject]
    private IDoctorService _doctorService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        Doctors = (await _doctorService.GetAllDoctorsAsync()).ToList();

        if (IsEditMode && id.HasValue)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id.Value);
            DepartmentDto = department;
        }
    }
}
