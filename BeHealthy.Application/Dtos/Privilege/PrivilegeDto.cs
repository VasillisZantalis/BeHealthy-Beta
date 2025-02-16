using BeHealthy.Domain;
using System.ComponentModel;

namespace BeHealthy.Application.Dtos.Privilege;

public class PrivilegeDto
{
    public PrivilegeName PrivilegeName { get; set; }
    public bool HasPrivilege { get; set; }
}
