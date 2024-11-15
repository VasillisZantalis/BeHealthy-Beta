using BeHealthy.Domain;
using BeHealthy.Domain.Entities;

namespace BeHealthy.Domain.Interfaces.Repositories;

public interface IPrivilegeRepository : IGenericRepository<Privilege>
{
    Task<Dictionary<string, bool>> GetUserPrivilegesAsync(UserRole userRole);
    Task<bool> HasPrivilegeAsync(UserRole role, string privilegeName);
}
