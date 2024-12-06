using BeHealthy.Domain.Interfaces;
using BeHealthy.Domain.Interfaces.Repositories;
using BeHealthy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BeHealthy.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

    public UnitOfWork(IDbContextFactory<ApplicationDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    private IPatientRepository? _patientRepository;
    private IDoctorRepository? _doctorRepository;
    private INurseRepository? _nurseRepository;
    private IAppointmentRepository? _appointmentRepository;
    private IDepartmentRepository? _departmentRepository;
    private IMedicalRecordRepository? _medicalRecordRepository;
    private IPrescriptionRepository? _prescriptionRepository;
    private IRoomRepository? _roomRepository;
    private IAppSettingsRepository? _appSettingsRepository;
    private IPrivilegeRepository? _privilegeRepository;
    private ISpecialtyRepository? _specialtyRepository;

    public IPatientRepository PatientRepository =>
        _patientRepository ??= new PatientRepository(_dbContextFactory);

    public IDoctorRepository DoctorRepository =>
        _doctorRepository ??= new DoctorRepository(_dbContextFactory);

    public INurseRepository NurseRepository =>
        _nurseRepository ??= new NurseRepository(_dbContextFactory);

    public IAppointmentRepository AppointmentRepository =>
        _appointmentRepository ??= new AppointmentRepository(_dbContextFactory);

    public IDepartmentRepository DepartmentRepository =>
        _departmentRepository ??= new DepartmentRepository(_dbContextFactory);

    public IMedicalRecordRepository MedicalRecordRepository =>
        _medicalRecordRepository ??= new MedicalRecordRepository(_dbContextFactory);

    public IPrescriptionRepository PrescriptionRepository =>
        _prescriptionRepository ??= new PrescriptionRepository(_dbContextFactory);

    public IRoomRepository RoomRepository =>
        _roomRepository ??= new RoomRepository(_dbContextFactory);

    public IAppSettingsRepository AppSettingsRepository =>
        _appSettingsRepository ??= new AppSettingsRepository(_dbContextFactory);

    public IPrivilegeRepository PrivilegeRepository =>
        _privilegeRepository ??= new PrivilegeRepository(_dbContextFactory);

    public ISpecialtyRepository SpecialtyRepository =>
        _specialtyRepository ??= new SpecialtyRepository(_dbContextFactory);

}
