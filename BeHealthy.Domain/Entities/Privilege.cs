using Microsoft.AspNetCore.Identity;

namespace BeHealthy.Domain.Entities;

public class Privilege
{
    public int Id { get; set; }
    public PrivilegeName Name { get; set; }
    public ICollection<UserRolePrivilege> UserRolePrivileges { get; set; }
}
