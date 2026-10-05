using BeHealthy.Domain.Entities;
using BeHealthy.Validation.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeHealthy.Infrastructure.Data.Configurations;

public class AppSettingConfiguration : IEntityTypeConfiguration<AppSetting>
{
    public void Configure(EntityTypeBuilder<AppSetting> builder)
    {
        builder.ToTable("AppSettings");

        builder.HasKey(x => x.Id);

        builder.Property(p => p.Key)
            .IsRequired()
            .HasMaxLength(FieldLengths.AppSettingKey);

        builder.Property(p => p.Caption)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(p => p.Type)
            .IsRequired();

        builder.Property(p => p.Group)
            .IsRequired();

        builder.Property(p => p.Value)
            .IsRequired();

        builder.Property(p => p.Description)
            .IsRequired();
    }
}
