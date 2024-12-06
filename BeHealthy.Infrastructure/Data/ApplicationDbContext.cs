using BeHealthy.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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
    }
}
