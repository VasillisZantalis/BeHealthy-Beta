using Microsoft.AspNetCore.Identity;

namespace BeHealthy.Shared.Models.Entities;

public class Privilege
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? DisplayName { get; set; }
    public bool Value { get; set; }
    public ICollection<RolePrivilege> RolePrivileges { get; set; } = new List<RolePrivilege>();
}
