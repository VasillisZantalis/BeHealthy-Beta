using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Domain;
using BeHealthy.Domain.Entities;
using BeHealthy.Domain.Interfaces;

namespace BeHealthy.Application.Services;

public class PrivilegeService : IPrivilegeService
{
    private readonly IUnitOfWork _unitOfWork;

    public PrivilegeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Dictionary<string, bool>> GetPrivilegesForRoleAsync(UserRole role)
    {
        var privileges = await _unitOfWork.PrivilegeRepository.GetUserPrivilegesAsync(role);

        return privileges;
    }

    public async Task<bool> HasPrivilege(UserRole role, string privilegeName)
    {
        return role == UserRole.Admin ? true : await _unitOfWork.PrivilegeRepository.HasPrivilegeAsync(role, privilegeName);
    }
}
