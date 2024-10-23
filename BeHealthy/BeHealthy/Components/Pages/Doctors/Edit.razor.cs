using BeHealthy.Shared.Interfaces;
using BeHealthy.Shared.Models.Dtos.Appointment;
using BeHealthy.Shared.Models.Dtos.Doctor;
using Microsoft.AspNetCore.Components;

namespace BeHealthy.Components.Pages.Doctors;

public partial class Edit
{
    [Parameter]
    public int DoctorId { get; set; }

    [SupplyParameterFromForm]
    private DoctorForUpdateDto _doctorForUpdateDto { get; set; } = new();

    [Inject]
    private IDoctorService _doctorService { get; set; } = default!;

    protected override async Task OnParametersSetAsync()
    {
        var doctor = await _doctorService.GetDoctorByIdAsync(DoctorId);
        ToDoctorForUpdateDto(doctor);
    }

    private void ToDoctorForUpdateDto(DoctorDto doctorDto)
    {
        _doctorForUpdateDto.Id = doctorDto.Id;
        _doctorForUpdateDto.FirstName = doctorDto.FirstName;
        _doctorForUpdateDto.LastName = doctorDto.LastName;
        _doctorForUpdateDto.Specialty = doctorDto.Specialty;
        _doctorForUpdateDto.PhoneNumber = doctorDto.PhoneNumber;
    }

    private void UpdateDoctor()
    {

    }
}
