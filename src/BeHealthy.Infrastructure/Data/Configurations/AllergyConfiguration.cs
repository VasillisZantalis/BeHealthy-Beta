using BeHealthy.Domain.Entities;
using BeHealthy.Validation.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeHealthy.Infrastructure.Data.Configurations;

public class AllergyConfiguration : IEntityTypeConfiguration<Allergy>
{
    public void Configure(EntityTypeBuilder<Allergy> builder)
    {
        builder.ToTable("Allergies");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.AllergyName)
            .IsRequired()
            .HasMaxLength(FieldLengths.AllergyName);

        builder.Property(a => a.Notes)
            .HasMaxLength(FieldLengths.AllergyNotes);

        builder.Property(a => a.Allergen)
            .IsRequired(false);

        builder.Property(a => a.Severity)
            .IsRequired();
    }
}