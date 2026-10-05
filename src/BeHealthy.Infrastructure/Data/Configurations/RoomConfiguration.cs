using BeHealthy.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BeHealthy.Infrastructure.Data.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("Rooms");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.Number)
            .IsRequired();

        // Relationships
        builder.HasMany(r => r.Appointments)
            .WithOne(a => a.Room)
            .HasForeignKey(a => a.RoomId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(r => r.Department)
            .WithMany(dept => dept.Rooms)
            .HasForeignKey(r => r.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
