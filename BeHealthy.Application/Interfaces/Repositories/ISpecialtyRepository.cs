using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Interfaces.Repositories;

public interface ISpecialtyRepository : IGenericRepository<Specialty>
{
    Task<List<Specialty>> GetAllSpecialtiesAsync();
}
