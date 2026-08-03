using System.ComponentModel;

namespace BeHealthy.Shared.Dtos.Privilege;

public class PrivilegeResponse
{
    public PrivilegeName PrivilegeName { get; set; }
    public bool HasPrivilege { get; set; }
}
