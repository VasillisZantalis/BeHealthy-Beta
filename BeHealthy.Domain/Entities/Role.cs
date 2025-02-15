namespace BeHealthy.Domain.Entities;

public class Role
{
    public short Id { get; set; }
    public UserRole Name { get; set; }
    public ICollection<UserRolePrivilege> UserRolePrivileges { get; set; } = new List<UserRolePrivilege>();
}