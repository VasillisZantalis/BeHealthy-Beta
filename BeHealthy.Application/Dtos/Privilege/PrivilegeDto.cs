using BeHealthy.Domain;
using System.ComponentModel;

namespace BeHealthy.Application.Dtos.Privilege;

public class PrivilegeDto
{
    public int Id { get; set; }
    public PrivilegeName Name { get; set; }
    public List<UserRole> Roles { get; set; }
    public bool Value { get; set; }
}
