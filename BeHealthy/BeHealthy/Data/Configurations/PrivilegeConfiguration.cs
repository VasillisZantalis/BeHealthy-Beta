using BeHealthy.Shared.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BeHealthy.Data.Configurations
{
    public class PrivilegeConfiguration : IEntityTypeConfiguration<Privilege>
    {
        public void Configure(EntityTypeBuilder<Privilege> builder)
        {
            builder
                .HasKey(p => p.Id);

            builder
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .Property(p => p.DisplayName)
                .HasMaxLength(200);
        }
    }
}
