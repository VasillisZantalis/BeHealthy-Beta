using BeHealthy.Domain;

namespace BeHealthy.Application.Dtos.Privilege;

public class PrivilegeDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? DisplayName { get; set; }
    public UserRole? RoleName { get; set; }
    public bool Value { get; set; }
}
