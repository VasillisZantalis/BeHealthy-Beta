using BeHealthy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeHealthy.Infrastructure.Data.Configurations;

public class DiagnosisConfiguration : IEntityTypeConfiguration<Diagnosis>
{
    public void Configure(EntityTypeBuilder<Diagnosis> builder)
    {
        builder.ToTable("Diagnoses");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(d => d.Notes)
            .HasMaxLength(1024);

        builder.HasOne(d => d.Visit)
            .WithMany(v => v.Diagnoses)
            .HasForeignKey(d => d.VisitId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(d => d.Treatments)
            .WithOne(t => t.Diagnosis)
            .HasForeignKey(t => t.DiagnosisId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}