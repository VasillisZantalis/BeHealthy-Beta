using BeHealthy.Application.Dtos.Privilege;
using BeHealthy.Domain;

namespace BeHealthy.Application.Services.Interfaces;

public interface IPrivilegeService
{
    Task<List<PrivilegeDto>> GetPrivilegesAsync();
    Task<List<PrivilegeDto>> GetPrivilegesForRoleAsync(UserRole role);
    Task<bool> HasPrivilege(UserRole role, string privilegeName);
    Task SavePrivilegesAsync(List<PrivilegeDto> privileges);
}
