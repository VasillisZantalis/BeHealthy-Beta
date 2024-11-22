using BeHealthy.Domain;
using BeHealthy.Domain.Entities;

namespace BeHealthy.Domain.Interfaces.Repositories;

public interface IPrivilegeRepository : IGenericRepository<Privilege>
{
    Task<List<Privilege>> GetPrivilegesAsync();
    Task<List<Privilege>> GetUserPrivilegesAsync(UserRole userRole);
    Task<bool> HasPrivilegeAsync(UserRole role, string privilegeName);
    Task UpdatePrivilegesAsync(List<Privilege> privileges);
}
