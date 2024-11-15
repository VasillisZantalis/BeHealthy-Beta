using BeHealthy.Domain;
using BeHealthy.Domain.Entities;

namespace BeHealthy.Domain.Interfaces.Repositories;

public interface IPrivilegeRepository : IGenericRepository<Privilege>
{
    Task<bool> HasPrivilegeAsync(UserRole role, string privilegeName);
}
