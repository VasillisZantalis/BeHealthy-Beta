using BeHealthy.Domain;

namespace BeHealthy.Application.Dtos.Privilege;

public class RolePrivilegesDto
{
    public UserRole Role { get; set; }
    public List<PrivilegeDto> Privileges { get; set; } = new();
}
