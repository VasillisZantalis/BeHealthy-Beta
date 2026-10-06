using BeHealthy.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BeHealthy.Validation.Common;
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
            .HasMaxLength(FieldLengths.Medication);

        builder.Property(p => p.Dosage)
            .IsRequired()
            .HasMaxLength(FieldLengths.Dosage);

        // A calendar date: stored and returned exactly as entered, with no time-zone conversion.
        builder.Property(p => p.DatePrescribed)
            .IsRequired();

        // Relationships. Prescriptions are clinical history: their patient or doctor can't be deleted.
        builder.HasOne(p => p.Patient)
            .WithMany()
            .HasForeignKey(p => p.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Doctor)
            .WithMany()
            .HasForeignKey(p => p.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Optional: the API can't create treatments, so a prescription is written without one.
        builder.HasOne(p => p.Treatment)
            .WithMany(t => t.Prescriptions)
            .HasForeignKey(p => p.TreatmentId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
    }
}