namespace BeHealthy.Domain.Entities;

public class UserRolePrivilege
{
    public short Id { get; set; }
    public Role? Role { get; set; }

    public int PrivilegeId { get; set; }
    public Privilege? Privilege { get; set; }

    public bool HasPrivilege { get; set; }
}
