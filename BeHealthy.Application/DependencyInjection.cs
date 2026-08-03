using System.Reflection;

namespace BeHealthy.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<INurseService, NurseService>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IMedicalRecordService, MedicalRecordService>();
        services.AddScoped<IPrescriptionService, PrescriptionService>();
        services.AddScoped<IRoomService, RoomService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAppSettingsService, AppSettingsService>();
        services.AddScoped<ISpecialtyService, SpecialtyService>();
        services.AddScoped<IVisitService, VisitService>();
        services.AddScoped<IAllergyService, AllergyService>();
        services.AddScoped<ISeedingService, SeedingService>();

        services.AddScoped<IValidatorService, ValidatorService>();

        return services;
    }

    public static IServiceCollection AddCQRS(
        this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddScoped<IDispatcher, Dispatcher>();

        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(classes => classes
                .AssignableTo(typeof(ICommandHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());


        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(classes => classes
                .AssignableTo(typeof(IQueryHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());


        return services;
    }
}
