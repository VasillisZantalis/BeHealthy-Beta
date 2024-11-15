using BeHealthy.Domain;
using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Services.Interfaces;

public interface IPrivilegeService
{
    Task<Dictionary<string, bool>> GetPrivilegesForRoleAsync(UserRole role);
    Task<bool> HasPrivilege(UserRole role, string privilegeName);
}
