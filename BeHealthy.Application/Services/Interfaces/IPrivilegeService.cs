using BeHealthy.Application.Dtos.Privilege;
using BeHealthy.Domain;

namespace BeHealthy.Application.Services.Interfaces;

public interface IPrivilegeService
{
    Task<List<RolePrivilegesDto>> GetPrivilegesAsync();
    Task<bool> HasPrivilegeAsync(UserRole role, PrivilegeName privilegeName);
    Task UpdatePrivilegeAsync(UserRole roleName, PrivilegeName privilegeName, bool hasPrivilege);
}
