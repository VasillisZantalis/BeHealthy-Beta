using BeHealthy.Application.Dtos.Privilege;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Domain;
using BeHealthy.Domain.Entities;
using BeHealthy.Domain.Interfaces;
using System.Data;

namespace BeHealthy.Application.Services;

public class PrivilegeService : IPrivilegeService
{
    private readonly IUnitOfWork _unitOfWork;

    public PrivilegeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<RolePrivilegesDto>> GetPrivilegesAsync()
    {
        var privileges = await _unitOfWork.PrivilegeRepository.GetPrivilegesAsync();

        var rolesWithPrivileges = privileges
            .SelectMany(p => p.UserRolePrivileges
                .Where(urp => urp.Role != null)
                .Select(urp => new { urp.Role, p.Name, urp.HasPrivilege }))
            .GroupBy(x => x.Role)
            .Select(group => new RolePrivilegesDto
            {
                Role = group.Key.Name,
                Privileges = group.Select(x => new PrivilegeDto
                {
                    PrivilegeName = x.Name,
                    HasPrivilege = x.HasPrivilege
                }).ToList()
            })
            .ToList();

        return rolesWithPrivileges;
    }

    public async Task<bool> HasPrivilegeAsync(UserRole userRole, PrivilegeName privilegeName)
    {
        if (userRole == UserRole.Admin)
            return true;

        var role = (await _unitOfWork.RoleRepository.FindAsync(r => r.Name == userRole)).FirstOrDefault();

        var privilege = (await _unitOfWork.PrivilegeRepository.FindAsync(p => p.Name == privilegeName)).FirstOrDefault();

        if (role == null || privilege == null)
        {
            return false;
        }

        var userRolePrivilege = await _unitOfWork.UserRolePrivilegeRepository.FindAsync(urp =>
           urp.Id == role.Id && urp.PrivilegeId == privilege.Id);

        return userRolePrivilege?.FirstOrDefault()?.HasPrivilege ?? false;
    }

    public async Task UpdatePrivilegeAsync(UserRole roleName, PrivilegeName privilegeName, bool hasPrivilege)
    {
        await _unitOfWork.PrivilegeRepository.UpdatePrivilegeAsync(roleName, privilegeName, hasPrivilege);

    }
}
