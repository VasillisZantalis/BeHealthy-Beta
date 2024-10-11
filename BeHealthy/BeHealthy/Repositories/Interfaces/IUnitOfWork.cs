namespace BeHealthy.Repositories.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IPatientRepository PatientRepository { get; }
    IDoctorRepository DoctorRepository { get; }
    INurseRepository NurseRepository { get; }
    IAppointmentRepository AppointmentRepository { get; }
    IDepartmentRepository DepartmentRepository { get; }
    IMedicalRecordRepository MedicalRecordRepository { get; }
    IPrescriptionRepository PrescriptionRepository { get; }
    IRoomRepository RoomRepository { get; }

    Task SaveChangesAsync();
}
