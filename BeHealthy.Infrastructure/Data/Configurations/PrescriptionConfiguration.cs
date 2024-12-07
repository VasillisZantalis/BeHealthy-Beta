using BeHealthy.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BeHealthy.Infrastructure.Data.Configurations;

public class PrescriptionConfiguration : IEntityTypeConfiguration<Prescription>
{
    public void Configure(EntityTypeBuilder<Prescription> builder)
    {
        builder.ToTable("Prescriptions");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Medication)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Dosage)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.DatePrescribed)
            .IsRequired()
            .HasConversion(
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                v => DateTime.SpecifyKind(v, DateTimeKind.Local)
            ); ;

        // Relationships
        builder.HasOne(p => p.Patient)
            .WithMany()
            .HasForeignKey(p => p.PatientId);

        builder.HasOne(p => p.Doctor)
            .WithMany()
            .HasForeignKey(p => p.DoctorId);
    }
}