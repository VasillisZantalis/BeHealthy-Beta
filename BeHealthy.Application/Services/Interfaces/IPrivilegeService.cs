using BeHealthy.Application.Dtos.Privilege;
using BeHealthy.Domain;

namespace BeHealthy.Application.Services.Interfaces;

public interface IPrivilegeService
{
    Task<List<PrivilegeDto>> GetPrivilegesAsync();
    Task<bool> HasPrivilegeAsync(UserRole role, PrivilegeName privilegeName);
    Task SavePrivilegesAsync(List<PrivilegeDto> privileges);
    Task SavePrivilegesAsync(UserRole userRole, List<PrivilegeName> privilegeNames, bool hasPrivilege);
}
