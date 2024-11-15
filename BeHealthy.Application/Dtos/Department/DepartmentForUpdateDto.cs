using BeHealthy.Application.Dtos.Doctor;
using BeHealthy.Application.Dtos.Nurse;
using BeHealthy.Application.Dtos.Patient;
using BeHealthy.Application.Dtos.Room;

namespace BeHealthy.Application.Dtos.Department;

public class DepartmentForUpdateDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;

    public int? HeadOfDepartmentId { get; set; }
    public DoctorDto? HeadOfDepartment { get; set; }
    public ICollection<DoctorDto> Doctors { get; set; } = new List<DoctorDto>();
    public ICollection<NurseDto> Nurses { get; set; } = new List<NurseDto>();
    public ICollection<PatientDto> Patients { get; set; } = new List<PatientDto>();
    public ICollection<RoomDto> Rooms { get; set; } = new List<RoomDto>();
}
