using BeHealthy.Shared.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BeHealthy.Data.Configurations;

public class RolePrivilegeConfiguration : IEntityTypeConfiguration<RolePrivilege>
{
    public void Configure(EntityTypeBuilder<RolePrivilege> builder)
    {
        builder.HasKey(rp => new { rp.Role, rp.PrivilegeId });

        builder
            .Property(rp => rp.Role)
            .HasConversion<short>();

        builder
            .HasOne(rp => rp.Privilege)
            .WithMany(p => p.RolePrivileges)
            .HasForeignKey(rp => rp.PrivilegeId)
            .IsRequired();
    }
}
