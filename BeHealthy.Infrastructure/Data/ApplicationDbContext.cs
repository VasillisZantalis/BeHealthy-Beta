using BeHealthy.Domain;
using BeHealthy.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection;
using System.Reflection.Emit;

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

        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        //var identityRoles = Enum.GetValues(typeof(UserRole))
        //    .Cast<UserRole>()
        //    .Select(role => new IdentityRole
        //    {
        //        Id = ((short)role).ToString(),
        //        Name = role.ToString(),
        //        NormalizedName = role.ToString().ToUpper()
        //    })
        //.ToArray();
        //modelBuilder.Entity<IdentityRole>().HasData(identityRoles);

        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Id = ((short)UserRole.Admin).ToString(),
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole
            {
                Id = ((short)UserRole.Doctor).ToString(),
                Name = "Doctor",
                NormalizedName = "DOCTOR"
            },
            new IdentityRole
            {
                Id = ((short)UserRole.Nurse).ToString(),
                Name = "Nurse",
                NormalizedName = "NURSE"
            },
            new IdentityRole
            {
                Id = ((short)UserRole.Patient).ToString(),
                Name = "Patient",
                NormalizedName = "PATIENT"
            }
        );


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

        //var adminUser = new ApplicationUser
        //{
        //    Id = "admin",
        //    UserName = "admin@gmail.com",
        //    NormalizedUserName = "ADMIN@GMAIL.COM",
        //    Email = "admin@gmail.com",
        //    NormalizedEmail = "ADMIN@GMAIL.COM",
        //    FirstName = "Admin",
        //    LastName = "User",
        //    SecurityStamp = "87B685CA-B2AC-4415-B86B-0EA9036617F0",
        //    PasswordHash = "123456aA@"
        //};

        //var userRoles = new List<IdentityUserRole<string>>();

        //userRoles.Add(new IdentityUserRole<string> { UserId = adminUser.Id, RoleId = ((short)UserRole.Admin).ToString() });
        //modelBuilder.Entity<IdentityUserRole<string>>().HasData(userRoles);

        //modelBuilder.Entity<ApplicationUser>().HasData(adminUser);

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
