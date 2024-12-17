using BeHealthy.Domain;

namespace BeHealthy.Application.Dtos.Privilege;

public class PrivilegeDto
{
    public int Id { get; set; }
    public PrivilegeName Name { get; set; }
    public UserRole? RoleName { get; set; }
    public bool Value { get; set; }
}
