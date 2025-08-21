using BeHealthy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeHealthy.Infrastructure.Data.Configurations;

public class TreatmentConfiguration : IEntityTypeConfiguration<Treatment>
{
    public void Configure(EntityTypeBuilder<Treatment> builder)
    {
        builder.ToTable("Treatments");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Description)
            .IsRequired()
            .HasMaxLength(512);

        builder.Property(t => t.StartDate)
            .IsRequired();

        builder.HasOne(t => t.Diagnosis)
            .WithMany(d => d.Treatments)
            .HasForeignKey(t => t.DiagnosisId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(t => t.Prescriptions)
            .WithOne()
            .HasForeignKey("TreatmentId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}