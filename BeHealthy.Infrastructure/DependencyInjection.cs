using BeHealthy.Application.Interfaces;
using BeHealthy.Application.Interfaces.Repositories;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Domain.Entities;
using BeHealthy.Infrastructure.Data;
using BeHealthy.Infrastructure.Localization;
using BeHealthy.Infrastructure.Repositories;
using BeHealthy.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BeHealthy.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
               .AddEnvironmentVariables()
               .AddUserSecrets<ApplicationDbContextFactory>()
               .Build();

        var connectionString = configuration.GetConnectionString("Default");

        services.AddDbContextFactory<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<INurseRepository, NurseRepository>();
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
        services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IAppSettingsRepository, AppSettingsRepository>();
        services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();
        services.AddScoped<IAllergyRepository, AllergyRepository>();
        services.AddScoped<IVisitRepository, VisitRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped(typeof(ILoggerService<>), typeof(LoggerService<>));

        services.AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddErrorDescriber<LocalizedIdentityErrorDescriber>()
            .AddSignInManager<SignInManager<ApplicationUser>>()
            .AddDefaultTokenProviders();

        return services;
    }
}
