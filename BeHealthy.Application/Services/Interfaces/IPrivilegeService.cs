using BeHealthy.Application.Dtos.Privilege;
using BeHealthy.Domain;

namespace BeHealthy.Application.Services.Interfaces;

public interface IPrivilegeService
{
    Task<List<PrivilegeDto>> GetPrivilegesAsync();
    Task<Dictionary<PrivilegeName, bool>> GetPrivilegesForRoleAsync(UserRole role);
    Task<bool> HasPrivilege(UserRole role, PrivilegeName privilegeName);
    Task SavePrivilegesAsync(List<PrivilegeDto> privileges);
}
