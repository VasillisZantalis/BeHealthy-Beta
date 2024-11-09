namespace BeHealthy.Repositories.Interfaces;

public interface IUnitOfWork
{
    IPatientRepository PatientRepository { get; }
    IDoctorRepository DoctorRepository { get; }
    INurseRepository NurseRepository { get; }
    IAppointmentRepository AppointmentRepository { get; }
    IDepartmentRepository DepartmentRepository { get; }
    IMedicalRecordRepository MedicalRecordRepository { get; }
    IPrescriptionRepository PrescriptionRepository { get; }
    IRoomRepository RoomRepository { get; }
    IAppSettingsRepository AppSettingsRepository { get; }
    IPrivilegeRepository PrivilegeRepository { get; }

}
