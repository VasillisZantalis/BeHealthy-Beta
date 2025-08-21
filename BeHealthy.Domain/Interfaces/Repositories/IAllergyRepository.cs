using BeHealthy.Domain.Entities;

namespace BeHealthy.Domain.Interfaces.Repositories;

public interface IAllergyRepository : IGenericRepository<Allergy>
{
    Task<IEnumerable<Allergy>> GetAllergiesByPatientIdAsync(int patientId);
}