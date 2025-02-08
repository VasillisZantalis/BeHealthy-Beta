using BeHealthy.Domain;
using BeHealthy.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BeHealthy.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Patient> Patients { get; set; } = null!;
    public DbSet<Doctor> Doctors { get; set; } = null!;
    public DbSet<Nurse> Nurses { get; set; } = null!;
    public DbSet<Appointment> Appointments { get; set; } = null!;
    public DbSet<Prescription> Prescriptions { get; set; } = null!;
    public DbSet<Department> Departments { get; set; } = null!;
    public DbSet<Room> Rooms { get; set; } = null!;
    public DbSet<AppSetting> AppSettings { get; set; } = null!;
    public DbSet<Privilege> Privileges { get; set; } = null!;
    public DbSet<RolePrivilege> RolePrivileges { get; set; } = null!;
    public DbSet<Specialty> Specialities { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Seed Roles
        var roles = Enum.GetValues(typeof(UserRole))
            .Cast<UserRole>()
            .Select(role => new IdentityRole
            {
                Id = ((short)role).ToString(),
                Name = role.ToString(),
                NormalizedName = role.ToString().ToUpper()
            })
            .ToArray();

        modelBuilder.Entity<IdentityRole>().HasData(roles);

        // Seed Privileges
        modelBuilder.Entity<Privilege>().HasData(
            new Privilege { Id = 1, Name = PrivilegeName.EditAppointments },
            new Privilege { Id = 2, Name = PrivilegeName.DeleteAppointments }
        );

        modelBuilder.Entity<RolePrivilege>().HasData(
            new RolePrivilege { Role = UserRole.Doctor, PrivilegeId = 1 },
            new RolePrivilege { Role = UserRole.Doctor, PrivilegeId = 2 },
            new RolePrivilege { Role = UserRole.Patient, PrivilegeId = 1 },
            new RolePrivilege { Role = UserRole.Patient, PrivilegeId = 2 },
            new RolePrivilege { Role = UserRole.Nurse, PrivilegeId = 1 },
            new RolePrivilege { Role = UserRole.Nurse, PrivilegeId = 2 }
        );

        var passwordHasher = new PasswordHasher<ApplicationUser>();

        // Seed Admin
        var adminUser = new ApplicationUser
        {
            Id = "admin",
            UserName = "admin@gmail.com",
            NormalizedUserName = "ADMIN@GMAIL.COM",
            Email = "admin@gmail.com",
            NormalizedEmail = "ADMIN@GMAIL.COM",
            FirstName = "Admin",
            LastName = "User",
            SecurityStamp = Guid.NewGuid().ToString("D")
        };
        adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "123456aA@");

        var userRoles = new List<IdentityUserRole<string>>();

        userRoles.Add(new IdentityUserRole<string> { UserId = adminUser.Id, RoleId = ((short)UserRole.Admin).ToString() });

        // Seed Application Users
        var users = new List<ApplicationUser> { adminUser };
        var doctors = new List<Doctor>();
        var nurses = new List<Nurse>();
        var patients = new List<Patient>();

        for (int i = 1; i <= 5; i++)
        {
            var doctorUserId = Guid.NewGuid().ToString();
            var nurseUserId = Guid.NewGuid().ToString();
            var patientUserId = Guid.NewGuid().ToString();

            var doctorUser = new ApplicationUser
            {
                Id = doctorUserId,
                UserName = $"doctor{i}@hospital.com",
                NormalizedUserName = $"DOCTOR{i}@HOSPITAL.COM",
                Email = $"doctor{i}@hospital.com",
                NormalizedEmail = $"DOCTOR{i}@HOSPITAL.COM",
                FirstName = $"DoctorFirstName{i}",
                LastName = $"DoctorLastName{i}",
                SecurityStamp = Guid.NewGuid().ToString("D")
            };
            doctorUser.PasswordHash = passwordHasher.HashPassword(doctorUser, "123456aA@");

            doctors.Add(new Doctor
            {
                Id = i,
                FirstName = $"DoctorFirstName{i}",
                LastName = $"DoctorLastName{i}",
                UserId = doctorUserId
            });

            users.Add(doctorUser);
            userRoles.Add(new IdentityUserRole<string> { UserId = doctorUser.Id, RoleId = ((short)UserRole.Doctor).ToString() });

            var nurseUser = new ApplicationUser
            {
                Id = nurseUserId,
                UserName = $"nurse{i}@hospital.com",
                NormalizedUserName = $"NURSE{i}@HOSPITAL.COM",
                Email = $"nurse{i}@hospital.com",
                NormalizedEmail = $"NURSE{i}@HOSPITAL.COM",
                FirstName = $"NurseFirstName{i}",
                LastName = $"NurseLastName{i}",
                SecurityStamp = Guid.NewGuid().ToString("D")
            };
            nurseUser.PasswordHash = passwordHasher.HashPassword(nurseUser, "123456aA@");

            nurses.Add(new Nurse
            {
                Id = i,
                FirstName = $"NurseFirstName{i}",
                LastName = $"NurseLastName{i}",
                UserId = nurseUserId
            });

            users.Add(nurseUser);
            userRoles.Add(new IdentityUserRole<string> { UserId = nurseUser.Id, RoleId = ((short)UserRole.Nurse).ToString() });

            var patientUser = new ApplicationUser
            {
                Id = patientUserId,
                UserName = $"patient{i}@hospital.com",
                NormalizedUserName = $"PATIENT{i}@HOSPITAL.COM",
                Email = $"patient{i}@hospital.com",
                NormalizedEmail = $"PATIENT{i}@HOSPITAL.COM",
                FirstName = $"PatientFirstName{i}",
                LastName = $"PatientLastName{i}",
                SecurityStamp = Guid.NewGuid().ToString("D")
            };
            patientUser.PasswordHash = passwordHasher.HashPassword(patientUser, "123456aA@");

            patients.Add(new Patient
            {
                Id = i,
                FirstName = $"PatientFirstName{i}",
                LastName = $"PatientLastName{i}",
                UserId = patientUserId
            });

            users.Add(patientUser);
            userRoles.Add(new IdentityUserRole<string> { UserId = patientUser.Id, RoleId = ((short)UserRole.Nurse).ToString() });

        }
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(userRoles);

        modelBuilder.Entity<ApplicationUser>().HasData(users);
        modelBuilder.Entity<Doctor>().HasData(doctors);
        modelBuilder.Entity<Nurse>().HasData(nurses);
        modelBuilder.Entity<Patient>().HasData(patients);

        // Seed Appointments
        var appointments = new List<Appointment>
        {
            new Appointment { Id = 1, AppointmentDate = new DateTime(2025, 2, 9, 10, 0, 0), Duration = 60, Status = AppointmentStatus.Scheduled, Reason = AppointmentReason.GeneralCheckup, DoctorId = 1, PatientId = 1 },
            new Appointment { Id = 2, AppointmentDate = new DateTime(2025, 2, 9, 11, 0, 0), Duration = 60, Status = AppointmentStatus.Scheduled, Reason = AppointmentReason.FollowUp, DoctorId = 2, PatientId = 2 },
            new Appointment { Id = 3, AppointmentDate = new DateTime(2025, 2, 9, 12, 0, 0), Duration = 60, Status = AppointmentStatus.Scheduled, Reason = AppointmentReason.Illness, DoctorId = 3, PatientId = 3 },
            new Appointment { Id = 4, AppointmentDate = new DateTime(2025, 2, 9, 13, 0, 0), Duration = 60, Status = AppointmentStatus.Scheduled, Reason = AppointmentReason.Injury, DoctorId = 4, PatientId = 4 },
            new Appointment { Id = 5, AppointmentDate = new DateTime(2025, 2, 9, 14, 0, 0), Duration = 60, Status = AppointmentStatus.Scheduled, Reason = AppointmentReason.Prescription, DoctorId = 5, PatientId = 5 },
            new Appointment { Id = 6, AppointmentDate = new DateTime(2025, 2, 10, 10, 0, 0), Duration = 60, Status = AppointmentStatus.Scheduled, Reason = AppointmentReason.GeneralCheckup, DoctorId = 1, PatientId = 2 },
            new Appointment { Id = 7, AppointmentDate = new DateTime(2025, 2, 10, 11, 0, 0), Duration = 60, Status = AppointmentStatus.Scheduled, Reason = AppointmentReason.FollowUp, DoctorId = 2, PatientId = 3 },
            new Appointment { Id = 8, AppointmentDate = new DateTime(2025, 2, 10, 12, 0, 0), Duration = 60, Status = AppointmentStatus.Scheduled, Reason = AppointmentReason.Illness, DoctorId = 3, PatientId = 4 },
            new Appointment { Id = 9, AppointmentDate = new DateTime(2025, 2, 10, 13, 0, 0), Duration = 60, Status = AppointmentStatus.Scheduled, Reason = AppointmentReason.Injury, DoctorId = 4, PatientId = 5 },
            new Appointment { Id = 10, AppointmentDate = new DateTime(2025, 2, 10, 14, 0, 0), Duration = 60, Status = AppointmentStatus.Scheduled, Reason = AppointmentReason.Prescription, DoctorId = 5, PatientId = 1 }
        };

        modelBuilder.Entity<Appointment>().HasData(appointments);
    }

}
