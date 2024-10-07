using BeHealthy.Shared.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeHealthy.Data.Configurations;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.ToTable("Doctors");

        builder.HasKey(x => x.Id);

        builder.Property(p => p.FirstName)
            .IsRequired();

        builder.Property(p => p.LastName)
            .IsRequired();

        builder.Property(p => p.CreatedAt)
            .IsRequired();
    }
}
