using BeHealthy.Domain;
using BeHealthy.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BeHealthy.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Nurse> Nurses => Set<Nurse>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Prescription> Prescriptions => Set<Prescription>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<AppSetting> AppSettings => Set<AppSetting>();
    public DbSet<Specialty> Specialties => Set<Specialty>();
    public DbSet<MedicalRecord> MedicalRecords => Set<MedicalRecord>();
    public DbSet<Visit> Visits => Set<Visit>();
    public DbSet<Allergy> Allergies => Set<Allergy>();
    public DbSet<LabResult> LabResults => Set<LabResult>();
    public DbSet<Diagnosis> Diagnoses => Set<Diagnosis>();
    public DbSet<Treatment> Treatments => Set<Treatment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Id = "0", // Admin
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole
            {
                Id = "2", // Doctor
                Name = "Doctor",
                NormalizedName = "DOCTOR"
            },
            new IdentityRole
            {
                Id = "3", // Nurse
                Name = "Nurse",
                NormalizedName = "NURSE"
            },
            new IdentityRole
            {
                Id = "4", // Patient
                Name = "Patient",
                NormalizedName = "PATIENT"
            }
        );

        modelBuilder.Entity<AppSetting>().HasData(
           new AppSetting
           {
               Id = 1,
               Key = "AppointmentRequiresRoom",
               Type = SettingType.Checkbox,
               Group = SettingGroup.Appointment,
               Value = "false",
               Caption = "Requires Room for Appointment",
               Description = "Indicates if a room is required for an appointment."
           },
           new AppSetting
           {
               Id = 2,
               Key = "DoNotAllowDoctorWithoutSpecialty",
               Type = SettingType.Checkbox,
               Group = SettingGroup.Doctor,
               Value = "false",
               Caption = "Do not allow doctors without a specialty",
               Description = "Indicates if doctors without a specialty should be allowed."
           },
           new AppSetting
           {
               Id = 3,
               Key = "DepartmentRequiresSupervisor",
               Type = SettingType.Checkbox,
               Group = SettingGroup.Department,
               Value = "false",
               Caption = "Department requires supervisor",
               Description = "Indicates if a department requires a supervisor."
           },
           new AppSetting
           {
               Id = 4,
               Key = "DefaultDepartmentSupervison",
               Type = SettingType.SingleSelect,
               Group = SettingGroup.Department,
               Value = "0",
               Caption = "Default Department Supervision",
               Description = "The default supervisor selection for departments."
           }
       );
    }
}
