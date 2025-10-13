using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Interfaces.Repositories;

public interface IPrescriptionRepository : IGenericRepository<Prescription>
{
    Task<IEnumerable<Prescription>> GetPrescriptionsByPatientIdAsync(int id);
}
