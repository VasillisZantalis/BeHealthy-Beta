using BeHealthy.Domain;
using BeHealthy.Domain.Entities;

namespace BeHealthy.Domain.Interfaces.Repositories;

public interface IPrivilegeRepository : IGenericRepository<Privilege>
{
    Task<List<Privilege>> GetPrivilegesAsync();
    Task<bool> HasPrivilegeAsync(UserRole role, PrivilegeName privilegeName);
    Task UpdatePrivilegesAsync(List<Privilege> privileges);
    Task UpdatePrivilegeAsync(UserRole roleName, PrivilegeName privilegeName, bool hasPrivilege);
}
