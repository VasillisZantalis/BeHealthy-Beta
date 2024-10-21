using BeHealthy.Shared.Models.Dtos.Doctor;
using BeHealthy.Shared.Models.Dtos.Nurse;
using BeHealthy.Shared.Models.Dtos.Patient;
using BeHealthy.Shared.Models.Dtos.Room;

namespace BeHealthy.Shared.Models.Dtos.Department;

public class DepartmentForCreationDto
{
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public int? HeadOfDepartmentId { get; set; }
    public DoctorDto? HeadOfDepartment { get; set; }
    public ICollection<DoctorDto> Doctors { get; set; } = new List<DoctorDto>();
    public ICollection<NurseDto> Nurses { get; set; } = new List<NurseDto>();
    public ICollection<PatientDto> Patients { get; set; } = new List<PatientDto>();
    public ICollection<RoomDto> Rooms { get; set; } = new List<RoomDto>();
}
