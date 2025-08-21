using BeHealthy.Domain.Entities;
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
            .HasMaxLength(256);

        builder.Property(v => v.Notes)
            .HasMaxLength(1024);

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
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(v => v.Diagnoses)
            .WithOne(d => d.Visit)
            .HasForeignKey(d => d.VisitId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(v => v.LabResults)
            .WithOne(lr => lr.Visit)
            .HasForeignKey(lr => lr.VisitId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(v => v.Treatments)
            .WithOne(t => t.Visit)
            .HasForeignKey(t => t.VisitId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}