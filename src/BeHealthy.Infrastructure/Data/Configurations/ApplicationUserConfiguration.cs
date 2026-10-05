using BeHealthy.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BeHealthy.Validation.Common;
using Microsoft.EntityFrameworkCore;

namespace BeHealthy.Infrastructure.Data.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("ApplicationUsers");

        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(FieldLengths.PersonName);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(FieldLengths.PersonName);

        builder.Property(u => u.DateOfBirth)
            .IsRequired(false)
            .HasConversion(
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : (DateTime?)null,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Local) : (DateTime?)null
            );

        builder.Property(u => u.Gender)
            .HasMaxLength(FieldLengths.Gender);

        builder.Property(u => u.Address)
            .HasMaxLength(FieldLengths.Address);
    }
}