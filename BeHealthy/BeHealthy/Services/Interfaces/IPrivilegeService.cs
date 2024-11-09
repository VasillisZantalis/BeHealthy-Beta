using BeHealthy.Shared.Models;
using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Services.Interfaces;

public interface IPrivilegeService
{
    Task<List<Privilege>> GetPrivilegesForRoleAsync(UserRole role);
    Task<bool> HasPrivilege(UserRole role, string privilegeName);
}
