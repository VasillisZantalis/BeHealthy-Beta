using BeHealthy.Domain.Entities;
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
            .HasMaxLength(128);

        builder.Property(a => a.Notes)
            .HasMaxLength(512);

        builder.Property(a => a.Allergen)
            .IsRequired(false);

        builder.Property(a => a.Severity)
            .IsRequired();
    }
}