using BeHealthy.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BeHealthy.Validation.Common;
using Microsoft.EntityFrameworkCore;

namespace BeHealthy.Infrastructure.Data.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Departments");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(FieldLengths.DepartmentName);

        builder.Property(d => d.Location)
            .IsRequired()
            .HasMaxLength(FieldLengths.DepartmentLocation);

        builder.HasOne(d => d.HeadOfDepartment)
           .WithMany()
           .HasForeignKey(d => d.HeadOfDepartmentId)
           .OnDelete(DeleteBehavior.SetNull);
    }
}
