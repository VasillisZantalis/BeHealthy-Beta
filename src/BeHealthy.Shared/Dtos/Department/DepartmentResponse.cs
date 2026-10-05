using BeHealthy.Shared.Dtos.Doctor;
using BeHealthy.Shared.Dtos.Nurse;
using BeHealthy.Shared.Dtos.Patient;
using BeHealthy.Shared.Dtos.Room;

namespace BeHealthy.Shared.Dtos.Department;

public class DepartmentResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public int? HeadOfDepartmentId { get; set; }
    public DoctorResponse? HeadOfDepartment { get; set; }
    public ICollection<DoctorResponse> Doctors { get; set; } = new List<DoctorResponse>();
    public ICollection<NurseResponse> Nurses { get; set; } = new List<NurseResponse>();
    public ICollection<PatientResponse> Patients { get; set; } = new List<PatientResponse>();
    public ICollection<RoomResponse> Rooms { get; set; } = new List<RoomResponse>();
}
