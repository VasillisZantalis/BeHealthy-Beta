using BeHealthy.Domain.Entities;
using BeHealthy.Validation.Common;
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
            .IsRequired();

        builder.Property(a => a.AppointmentStartTime)
            .IsRequired();

        builder.Property(a => a.AppointmentEndTime)
            .IsRequired();

        builder.Property(a => a.Notes)
            .HasMaxLength(FieldLengths.AppointmentNotes);

        // Relationships. Appointments are clinical history: a patient or doctor who has any can't be deleted.
        builder.HasOne(a => a.Patient)
            .WithMany(p => p.Appointments)
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Doctor)
            .WithMany(d => d.Appointments)
            .HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

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
