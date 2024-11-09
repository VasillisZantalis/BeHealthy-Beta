using Microsoft.AspNetCore.Identity;

namespace BeHealthy.Shared.Models.Entities;

public class RolePrivilege
{
    public UserRole Role { get; set; }
    public int PrivilegeId { get; set; }
    public Privilege? Privilege { get; set; }
}
