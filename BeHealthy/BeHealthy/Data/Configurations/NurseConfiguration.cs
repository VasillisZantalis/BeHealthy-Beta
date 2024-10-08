using BeHealthy.Shared.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BeHealthy.Data.Configurations;

public class NurseConfiguration : IEntityTypeConfiguration<Nurse>
{
    public void Configure(EntityTypeBuilder<Nurse> builder)
    {
        builder.ToTable("Nurses");

        builder.HasKey(n => n.Id);

        builder.HasOne(n => n.Department)
            .WithMany(dept => dept.Nurses)
            .HasForeignKey(n => n.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(d => d.UserId)
            .IsRequired();

        builder.HasOne(d => d.User)
            .WithOne(u => u.Nurse)
            .HasForeignKey<Nurse>(d => d.UserId);

        builder.HasOne(d => d.Department)
            .WithMany(d => d.Nurses)
            .HasForeignKey(d => d.DepartmentId);
    }
}
