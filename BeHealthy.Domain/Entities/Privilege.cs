using Microsoft.AspNetCore.Identity;

namespace BeHealthy.Domain.Entities;

public class Privilege
{
    public int Id { get; set; }
    public PrivilegeName Name { get; set; }
    public bool Value { get; set; }
    public ICollection<RolePrivilege> RolePrivileges { get; set; } = new List<RolePrivilege>();
}
