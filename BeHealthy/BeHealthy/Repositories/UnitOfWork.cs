using BeHealthy.Data;
using BeHealthy.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeHealthy.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    private IPatientRepository? _patientRepository;
    private IDoctorRepository? _doctorRepository;
    private INurseRepository? _nurseRepository;
    private IAppointmentRepository? _appointmentRepository;
    private IDepartmentRepository? _departmentRepository;
    private IMedicalRecordRepository? _medicalRecordRepository;
    private IPrescriptionRepository? _prescriptionRepository;
    private IRoomRepository? _roomRepository;

    public IPatientRepository PatientRepository =>
        _patientRepository ??= new PatientRepository(_context);

    public IDoctorRepository DoctorRepository =>
        _doctorRepository ??= new DoctorRepository(_context);

    public INurseRepository NurseRepository =>
        _nurseRepository ??= new NurseRepository(_context);

    public IAppointmentRepository AppointmentRepository =>
        _appointmentRepository ??= new AppointmentRepository(_context);

    public IDepartmentRepository DepartmentRepository =>
        _departmentRepository ??= new DepartmentRepository(_context);

    public IMedicalRecordRepository MedicalRecordRepository =>
        _medicalRecordRepository ??= new MedicalRecordRepository(_context);

    public IPrescriptionRepository PrescriptionRepository =>
        _prescriptionRepository ??= new PrescriptionRepository(_context);

    public IRoomRepository RoomRepository =>
        _roomRepository ??= new RoomRepository(_context);

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
