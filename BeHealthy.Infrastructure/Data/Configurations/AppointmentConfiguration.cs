using BeHealthy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeHealthy.Infrastructure.Data.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Appointments");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.AppointmentDate)
            .IsRequired()
            .HasConversion(
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc), // Convert to UTC before saving
                v => DateTime.SpecifyKind(v, DateTimeKind.Local) // Read as Local Time
            ); 

        builder.Property(a => a.Notes)
            .HasMaxLength(500);

        // Relationships
        builder.HasOne(a => a.Patient)
            .WithMany(p => p.Appointments)
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Doctor)
            .WithMany(d => d.Appointments)
            .HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Nurse)
            .WithMany(d => d.Appointments)
            .HasForeignKey(a => a.NurseId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(a => a.Room)
            .WithMany(r => r.Appointments)
            .HasForeignKey(a => a.RoomId)
            .OnDelete(DeleteBehavior.SetNull);

    }
}
