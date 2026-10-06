using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Interfaces.Repositories;

public interface IDoctorRepository : IGenericRepository<Doctor>
{

    Task<IEnumerable<Doctor>> GetAllDoctorsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Doctor>> GetAllDoctorsSimpleAsync(CancellationToken cancellationToken = default);
    Task<Doctor?> GetDoctorByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<bool> DeleteDoctorAsync(int id, CancellationToken cancellationToken = default);
    /// <summary>True when the doctor has appointments, visits or prescriptions, which must be kept.</summary>
    Task<bool> HasClinicalHistoryAsync(int doctorId, CancellationToken cancellationToken = default);
    Task<bool> IsDoctorHeadOfDepartmentAsync(int doctorId, CancellationToken cancellationToken = default);
}
