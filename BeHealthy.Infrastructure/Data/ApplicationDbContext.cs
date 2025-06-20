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
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Privilege> Privileges => Set<Privilege>();
    public DbSet<UserRolePrivilege> UserRolePrivileges => Set<UserRolePrivilege>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Seed Roles
        var identityRoles = Enum.GetValues(typeof(UserRole))
            .Cast<UserRole>()
            .Select(role => new IdentityRole
            {
                Id = ((short)role).ToString(),
                Name = role.ToString(),
                NormalizedName = role.ToString().ToUpper()
            })
            .ToArray();

        modelBuilder.Entity<IdentityRole>().HasData(identityRoles);

        // Seed Privileges
        var privileges = new List<Privilege>
        {
            new Privilege { Id = 1, Name = PrivilegeName.EditAppointments },
            new Privilege { Id = 2, Name = PrivilegeName.DeleteAppointments },
            new Privilege { Id = 3, Name = PrivilegeName.EditPatient },
            new Privilege { Id = 4, Name = PrivilegeName.DeletePatient },
            new Privilege { Id = 5, Name = PrivilegeName.PrescribeMedications },
            new Privilege { Id = 6, Name = PrivilegeName.ViewPatientPrescriptions },
            new Privilege { Id = 7, Name = PrivilegeName.GenerateMedicalReports }
        };

        // Add the privileges to the model
        modelBuilder.Entity<Privilege>().HasData(privileges);


        // Seed Roles
        var roles = new List<Role>
        {
            new Role { Id = 1, Name = UserRole.Admin },
            new Role { Id = 2, Name = UserRole.Staff },
            new Role { Id = 3, Name = UserRole.Doctor },
            new Role { Id = 4, Name = UserRole.Nurse },
            new Role { Id = 5, Name = UserRole.Patient }
        };

        // Add the roles to the model
        modelBuilder.Entity<Role>().HasData(roles);


        var userRolePrivileges = new List<UserRolePrivilege>();

        foreach (var role in roles)
        {
            foreach (var privilege in privileges)
            {
                userRolePrivileges.Add(new UserRolePrivilege
                {
                    Id = role.Id,
                    PrivilegeId = privilege.Id,
                    HasPrivilege = false
                });
            }
        }

        modelBuilder.Entity<UserRolePrivilege>().HasData(userRolePrivileges);

        var passwordHasher = new PasswordHasher<ApplicationUser>();

        // Seed Departments
        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, Name = "Cardiology", Location = "Building A - Floor 3", CreatedAt = DateTime.UtcNow },
            new Department { Id = 2, Name = "Neurology", Location = "Building B - Floor 2", CreatedAt = DateTime.UtcNow },
            new Department { Id = 3, Name = "Orthopedics", Location = "Building C - Floor 1", CreatedAt = DateTime.UtcNow },
            new Department { Id = 4, Name = "Pediatrics", Location = "Building D - Floor 4", CreatedAt = DateTime.UtcNow },
            new Department { Id = 5, Name = "Emergency", Location = "Building E - Ground Floor", CreatedAt = DateTime.UtcNow }
        );

        // Seed Rooms
        modelBuilder.Entity<Room>().HasData(
            new Room { Id = 1, Name = "Room 301", Number = 301, CreatedAt = DateTime.UtcNow, DepartmentId = 1 },
            new Room { Id = 2, Name = "Room 302", Number = 302, CreatedAt = DateTime.UtcNow, DepartmentId = 1 },
            new Room { Id = 3, Name = "Room 201", Number = 201, CreatedAt = DateTime.UtcNow, DepartmentId = 2 },
            new Room { Id = 4, Name = "Room 202", Number = 202, CreatedAt = DateTime.UtcNow, DepartmentId = 2 },
            new Room { Id = 5, Name = "Room 101", Number = 101, CreatedAt = DateTime.UtcNow, DepartmentId = 3 },
            new Room { Id = 6, Name = "Room 102", Number = 102, CreatedAt = DateTime.UtcNow, DepartmentId = 3 },
            new Room { Id = 7, Name = "Room 401", Number = 401, CreatedAt = DateTime.UtcNow, DepartmentId = 4 },
            new Room { Id = 8, Name = "Room 402", Number = 402, CreatedAt = DateTime.UtcNow, DepartmentId = 4 },
            new Room { Id = 9, Name = "Emergency Room 1", Number = 1, CreatedAt = DateTime.UtcNow, DepartmentId = 5 },
            new Room { Id = 10, Name = "Emergency Room 2", Number = 2, CreatedAt = DateTime.UtcNow, DepartmentId = 5 }
        );

        // Seed Specialties
        modelBuilder.Entity<Specialty>().HasData(
            new Specialty { Id = 1, Name = "Cardiology", CreatedAt = DateTime.UtcNow },
            new Specialty { Id = 2, Name = "Neurology", CreatedAt = DateTime.UtcNow },
            new Specialty { Id = 3, Name = "Orthopedics", CreatedAt = DateTime.UtcNow },
            new Specialty { Id = 4, Name = "Pediatrics", CreatedAt = DateTime.UtcNow },
            new Specialty { Id = 5, Name = "Emergency Medicine", CreatedAt = DateTime.UtcNow },
            new Specialty { Id = 6, Name = "Radiology", CreatedAt = DateTime.UtcNow },
            new Specialty { Id = 7, Name = "Oncology", CreatedAt = DateTime.UtcNow },
            new Specialty { Id = 8, Name = "Dermatology", CreatedAt = DateTime.UtcNow },
            new Specialty { Id = 9, Name = "General Surgery", CreatedAt = DateTime.UtcNow },
            new Specialty { Id = 10, Name = "Anesthesiology", CreatedAt = DateTime.UtcNow }
        );

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

        for (int i = 1; i <= 15; i++)
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
            userRoles.Add(new IdentityUserRole<string> { UserId = patientUser.Id, RoleId = ((short)UserRole.Patient).ToString() });

        }
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(userRoles);

        modelBuilder.Entity<ApplicationUser>().HasData(users);
        modelBuilder.Entity<Doctor>().HasData(doctors);
        modelBuilder.Entity<Nurse>().HasData(nurses);
        modelBuilder.Entity<Patient>().HasData(patients);

        var today = DateOnly.FromDateTime(DateTime.Now.Date);

        // Seed Appointments
        var appointments = new List<Appointment>
        {
            new Appointment { Id = 1, AppointmentDate = today.AddDays(-5), AppointmentStartTime = new TimeOnly(10, 0), AppointmentEndTime = new TimeOnly(11, 0), Status = AppointmentStatus.Completed, Reason = AppointmentReason.GeneralCheckup, DoctorId = 1, PatientId = 1 },
            new Appointment { Id = 2, AppointmentDate = today.AddDays(-3), AppointmentStartTime = new TimeOnly(11, 0), AppointmentEndTime = new TimeOnly(12, 0), Status = AppointmentStatus.Cancelled, Reason = AppointmentReason.FollowUp, DoctorId = 2, PatientId = 2 },
            new Appointment { Id = 3, AppointmentDate = today.AddDays(-2), AppointmentStartTime = new TimeOnly(9, 0), AppointmentEndTime = new TimeOnly(10, 0), Status = AppointmentStatus.Cancelled, Reason = AppointmentReason.FollowUp, DoctorId = 3, PatientId = 3 },
            new Appointment { Id = 4, AppointmentDate = today.AddDays(-1), AppointmentStartTime = new TimeOnly(12, 0), AppointmentEndTime = new TimeOnly(13, 0), Status = AppointmentStatus.Completed, Reason = AppointmentReason.FollowUp, DoctorId = 4, PatientId = 4 },

            // Today's Appointments
            new Appointment { Id = 5, AppointmentDate = today, AppointmentStartTime = new TimeOnly(12, 0), AppointmentEndTime = new TimeOnly(13, 0), Status = AppointmentStatus.Scheduled, Reason = AppointmentReason.Illness, DoctorId = 3, PatientId = 3 },
            new Appointment { Id = 6, AppointmentDate = today, AppointmentStartTime = new TimeOnly(14, 0), AppointmentEndTime = new TimeOnly(15, 0), Status = AppointmentStatus.Rescheduled, Reason = AppointmentReason.Injury, DoctorId = 4, PatientId = 4 },
            new Appointment { Id = 7, AppointmentDate = today, AppointmentStartTime = new TimeOnly(9, 0), AppointmentEndTime = new TimeOnly(10, 0), Status = AppointmentStatus.Scheduled, Reason = AppointmentReason.Injury, DoctorId = 4, PatientId = 1 },
            new Appointment { Id = 8, AppointmentDate = today, AppointmentStartTime = new TimeOnly(11, 0), AppointmentEndTime = new TimeOnly(12, 0), Status = AppointmentStatus.Scheduled, Reason = AppointmentReason.Injury, DoctorId = 3, PatientId = 2 },
            new Appointment { Id = 9, AppointmentDate = today, AppointmentStartTime = new TimeOnly(15, 0), AppointmentEndTime = new TimeOnly(16, 0), Status = AppointmentStatus.Rescheduled, Reason = AppointmentReason.Injury, DoctorId = 1, PatientId = 1 },

            // Future Appointments
            new Appointment { Id = 10, AppointmentDate = today.AddDays(1), AppointmentStartTime = new TimeOnly(10, 0), AppointmentEndTime = new TimeOnly(11, 0), Status = AppointmentStatus.Scheduled, Reason = AppointmentReason.Prescription, DoctorId = 5, PatientId = 5 },
            new Appointment { Id = 11, AppointmentDate = today.AddDays(2), AppointmentStartTime = new TimeOnly(11, 0), AppointmentEndTime = new TimeOnly(12, 0), Status = AppointmentStatus.Rescheduled, Reason = AppointmentReason.GeneralCheckup, DoctorId = 1, PatientId = 2 },
            new Appointment { Id = 12, AppointmentDate = today.AddDays(3), AppointmentStartTime = new TimeOnly(12, 0), AppointmentEndTime = new TimeOnly(13, 0), Status = AppointmentStatus.Scheduled, Reason = AppointmentReason.FollowUp, DoctorId = 2, PatientId = 3 },

            // Further Future Appointments
            new Appointment { Id = 13, AppointmentDate = today.AddDays(10), AppointmentStartTime = new TimeOnly(13, 0), AppointmentEndTime = new TimeOnly(14, 0), Status = AppointmentStatus.Scheduled, Reason = AppointmentReason.Injury, DoctorId = 4, PatientId = 5 },
            new Appointment { Id = 14, AppointmentDate = today.AddDays(15), AppointmentStartTime = new TimeOnly(14, 0), AppointmentEndTime = new TimeOnly(15, 0), Status = AppointmentStatus.Rescheduled, Reason = AppointmentReason.Prescription, DoctorId = 5, PatientId = 1 }
        };

        modelBuilder.Entity<Appointment>().HasData(appointments);

        // Seed AppSettings

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
