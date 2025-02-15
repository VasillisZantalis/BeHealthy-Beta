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

    public async Task<List<PrivilegeDto>> GetPrivilegesAsync()
    {
        var privileges = await _unitOfWork.PrivilegeRepository.GetPrivilegesAsync();

        var privilegesDto = privileges.Select(p => new PrivilegeDto
        {
            Id = p.Id,
            Name = p.Name,
            Roles = p.UserRolePrivileges
            .Where(w => w.Role.Name != UserRole.Admin
                && w.Role.Name != UserRole.Staff)
            .Select(s => s.Role.Name).ToList(),
        }).ToList();

        return privilegesDto;
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

    public async Task SavePrivilegesAsync(List<PrivilegeDto> privileges)
    {
        var entities = privileges.Select(dto => new Privilege
        {
            Id = dto.Id,
            Name = dto.Name,
        }).ToList();

        await _unitOfWork.PrivilegeRepository.UpdatePrivilegesAsync(entities);
    }

    public Task SavePrivilegesAsync(UserRole userRole, List<PrivilegeName> privilegeNames, bool hasPrivilege)
    {
        throw new NotImplementedException();
    }
}
