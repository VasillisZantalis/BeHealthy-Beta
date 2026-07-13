using BeHealthy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeHealthy.Infrastructure.Data.Configurations;

public class MedicalRecordConfiguration : IEntityTypeConfiguration<MedicalRecord>
{
    public void Configure(EntityTypeBuilder<MedicalRecord> builder)
    {
        builder.ToTable("MedicalRecords");
        builder.HasKey(mr => mr.Id);

        builder.Property(mr => mr.CreatedUserId)
            .IsRequired(false);

        builder.Property(mr => mr.CreatedBy)
            .IsRequired(false);

        builder.Property(p => p.RecordDate)
            .IsRequired();

        builder.Property(mr => mr.Notes)
            .IsRequired(false)
            .HasMaxLength(500);
    }
}
