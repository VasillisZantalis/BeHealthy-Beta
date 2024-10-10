using BeHealthy.Shared.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeHealthy.Data.Configurations;

public class AppSettingConfiguration : IEntityTypeConfiguration<AppSetting>
{
    public void Configure(EntityTypeBuilder<AppSetting> builder)
    {
        builder.ToTable("AppSettings");

        builder.HasKey(x => x.Id);

        builder.Property(p => p.Name)
            .IsRequired();

        builder.Property(p => p.Area)
            .IsRequired();

        builder.Property(p => p.StringValue)
            .IsRequired(false);

        builder.Property(p => p.IntValue)
            .IsRequired(false);

        builder.Property(p => p.BoolValue)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(p => p.InsDate)
            .IsRequired()
            .HasDefaultValue(DateTime.UtcNow);

    }
}
