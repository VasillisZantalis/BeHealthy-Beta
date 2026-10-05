using BeHealthy.Application.Validators;
using BeHealthy.Validation;
using BeHealthy.Validation.Settings;
using FluentValidation;

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
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAppSettingsService, AppSettingsService>();
        services.AddScoped<ISpecialtyService, SpecialtyService>();
        services.AddScoped<IVisitService, VisitService>();
        services.AddScoped<IAllergyService, AllergyService>();
        services.AddScoped<ISeedingService, SeedingService>();

        // Validation: a request type can have two validators, and the API runs both.
        // - the shared one from BeHealthy.Validation (also used by the Blazor front end), and
        // - a "*ServerValidator" from this assembly for rules that need the database.
        services.AddSharedValidators();
        services.AddValidatorsFromAssemblyContaining<ValidationSettingsProvider>(ServiceLifetime.Scoped);
        services.AddScoped<IValidationSettingsProvider, ValidationSettingsProvider>();

        return services;
    }
}
