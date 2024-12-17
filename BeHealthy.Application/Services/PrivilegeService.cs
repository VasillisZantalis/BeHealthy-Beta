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
            Value = p.Value,
            RoleName = p.RolePrivileges.Select(s => s.Role).FirstOrDefault()
        }).ToList();

        return privilegesDto;
    }

    public async Task<Dictionary<PrivilegeName, bool>> GetPrivilegesForRoleAsync(UserRole role)
    {
        var privileges = await _unitOfWork.PrivilegeRepository.GetUserPrivilegesAsync(role);

        return privileges;
    }

    public async Task<bool> HasPrivilege(UserRole role, PrivilegeName privilegeName)
    {
        return role == UserRole.Admin ? true : await _unitOfWork.PrivilegeRepository.HasPrivilegeAsync(role, privilegeName);
    }

    public async Task SavePrivilegesAsync(List<PrivilegeDto> privileges)
    {
        var entities = privileges.Select(dto => new Privilege
        {
            Id = dto.Id,
            Name = dto.Name,
            Value = dto.Value
        }).ToList();

        await _unitOfWork.PrivilegeRepository.UpdatePrivilegesAsync(entities);
    }
}
