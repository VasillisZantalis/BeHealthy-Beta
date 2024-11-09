using BeHealthy.Shared.Models;
using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Repositories.Interfaces;

public interface IPrivilegeRepository : IGenericRepository<Privilege>
{
    Task<bool> HasPrivilegeAsync(UserRole role, string privilegeName);
}
