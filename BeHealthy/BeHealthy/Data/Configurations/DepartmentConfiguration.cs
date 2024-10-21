using BeHealthy.Shared.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BeHealthy.Data.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Departments");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.Location)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(d => d.HeadOfDepartment)
           .WithMany()
           .HasForeignKey(d => d.HeadOfDepartmentId)
           .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(d => d.Doctors)
            .WithOne(d => d.Department)
            .HasForeignKey(d => d.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(d => d.Nurses)
            .WithOne(n => n.Department)
            .HasForeignKey(n => n.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(d => d.Patients)
            .WithOne(p => p.Department)
            .HasForeignKey(p => p.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(d => d.Rooms)
            .WithOne(r => r.Department)
            .HasForeignKey(r => r.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
