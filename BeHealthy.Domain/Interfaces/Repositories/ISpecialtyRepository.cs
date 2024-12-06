using BeHealthy.Domain.Entities;

namespace BeHealthy.Domain.Interfaces.Repositories;

public interface ISpecialtyRepository : IGenericRepository<Specialty>
{
    Task<List<Specialty>> GetAllSpecialtiesAsync();
}
