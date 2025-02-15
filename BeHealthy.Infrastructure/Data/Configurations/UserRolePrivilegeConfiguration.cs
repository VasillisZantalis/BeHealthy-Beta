using BeHealthy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeHealthy.Infrastructure.Data.Configurations;

public class UserRolePrivilegeConfiguration : IEntityTypeConfiguration<UserRolePrivilege>
{
    public void Configure(EntityTypeBuilder<UserRolePrivilege> builder)
    {
        builder.ToTable("UserRolePrivileges");

        // Composite Key: RoleId and PrivilegeId
        builder.HasKey(urp => new { urp.Id, urp.PrivilegeId });

        // Relationship between UserRolePrivilege and Role
        builder.HasOne(urp => urp.Role)
            .WithMany(r => r.UserRolePrivileges)
            .HasForeignKey(urp => urp.Id);

        // Relationship between UserRolePrivilege and Privilege
        builder.HasOne(urp => urp.Privilege)
            .WithMany(p => p.UserRolePrivileges)
            .HasForeignKey(urp => urp.PrivilegeId);
    }
}
