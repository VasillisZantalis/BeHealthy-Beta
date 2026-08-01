using System.ComponentModel;

namespace BeHealthy.Shared.Dtos.Privilege;

public class PrivilegeDto
{
    public PrivilegeName PrivilegeName { get; set; }
    public bool HasPrivilege { get; set; }
}
