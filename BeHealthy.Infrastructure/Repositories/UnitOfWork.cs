using BeHealthy.Application.Interfaces;
using BeHealthy.Application.Interfaces.Repositories;
using BeHealthy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace BeHealthy.Infrastructure.Repositories;

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
    private IAppSettingsRepository? _appSettingsRepository;
    private ISpecialtyRepository? _specialtyRepository;
    private IAllergyRepository? _allergyRepository;
    private IVisitRepository? _visitRepository;

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

    public IAppSettingsRepository AppSettingsRepository =>
        _appSettingsRepository ??= new AppSettingsRepository(_context);

    public ISpecialtyRepository SpecialtyRepository =>
        _specialtyRepository ??= new SpecialtyRepository(_context);

    public IAllergyRepository AllergyRepository =>
        _allergyRepository ??= new AllergyRepository(_context);

    public IVisitRepository VisitRepository =>
        _visitRepository ??= new VisitRepository(_context);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();

    public async Task<IUnitOfWorkTransaction> BeginTransactionAsync()
    {
        var transaction = await _context.Database.BeginTransactionAsync();
        return new EfUnitOfWorkTransaction(transaction);
    }

    private sealed class EfUnitOfWorkTransaction : IUnitOfWorkTransaction
    {
        private readonly IDbContextTransaction _transaction;

        public EfUnitOfWorkTransaction(IDbContextTransaction transaction)
        {
            _transaction = transaction;
        }

        public Task CommitAsync() => _transaction.CommitAsync();

        public Task RollbackAsync() => _transaction.RollbackAsync();

        public ValueTask DisposeAsync() => _transaction.DisposeAsync();
    }
}
