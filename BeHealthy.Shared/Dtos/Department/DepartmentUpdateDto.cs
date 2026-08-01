using BeHealthy.Shared.Dtos.Doctor;
using BeHealthy.Shared.Dtos.Nurse;
using BeHealthy.Shared.Dtos.Patient;
using BeHealthy.Shared.Dtos.Room;

namespace BeHealthy.Shared.Dtos.Department;

public class DepartmentUpdateDto
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
