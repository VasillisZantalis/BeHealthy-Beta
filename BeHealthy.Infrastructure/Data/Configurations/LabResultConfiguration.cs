using BeHealthy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeHealthy.Infrastructure.Data.Configurations;

public class LabResultConfiguration : IEntityTypeConfiguration<LabResult>
{
    public void Configure(EntityTypeBuilder<LabResult> builder)
    {
        builder.ToTable("LabResults");

        builder.HasKey(lr => lr.Id);

        builder.Property(lr => lr.TestName)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(lr => lr.ResultValue)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(lr => lr.Unit)
            .HasMaxLength(64);

        builder.Property(lr => lr.ReferenceRange)
            .HasMaxLength(128);

        builder.Property(lr => lr.ResultDate)
            .IsRequired();

        builder.HasOne(lr => lr.Visit)
            .WithMany(v => v.LabResults)
            .HasForeignKey(lr => lr.VisitId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}