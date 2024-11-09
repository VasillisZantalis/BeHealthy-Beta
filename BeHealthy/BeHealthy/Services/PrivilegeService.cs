using BeHealthy.Repositories.Interfaces;
using BeHealthy.Services.Interfaces;
using BeHealthy.Shared.Models;
using BeHealthy.Shared.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BeHealthy.Services;

public class PrivilegeService : IPrivilegeService
{
    private readonly IUnitOfWork _unitOfWork;

    public PrivilegeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Privilege>> GetPrivilegesForRoleAsync(UserRole role)
    {
        var privileges = await _unitOfWork.PrivilegeRepository
            .GetQueryable()
            .Include(i => i.RolePrivileges)
            .Where(p => p.RolePrivileges.Any(rp => rp.Role == role))
            .ToListAsync();

        return privileges;
    }

    public async Task<bool> HasPrivilege(UserRole role, string privilegeName)
    {
        return role == UserRole.Admin ? true : await _unitOfWork.PrivilegeRepository.HasPrivilegeAsync(role, privilegeName); 
    }
}
