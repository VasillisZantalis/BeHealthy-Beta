using BeHealthy.Domain.Entities;
using BeHealthy.Validation.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeHealthy.Infrastructure.Data.Configurations;

public class VisitConfiguration : IEntityTypeConfiguration<Visit>
{
    public void Configure(EntityTypeBuilder<Visit> builder)
    {
        builder.ToTable("Visits");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.VisitDate)
            .IsRequired();

        builder.Property(v => v.Reason)
            .IsRequired()
            .HasMaxLength(FieldLengths.VisitReason);

        builder.Property(v => v.Notes)
            .HasMaxLength(FieldLengths.VisitNotes);

        builder.HasOne(v => v.Patient)
            .WithMany(p => p.Visits)
            .HasForeignKey(v => v.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(v => v.Doctor)
            .WithMany()
            .HasForeignKey(v => v.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(v => v.MedicalRecord)
            .WithMany(mr => mr.Visits)
            .HasForeignKey(v => v.MedicalRecordId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}