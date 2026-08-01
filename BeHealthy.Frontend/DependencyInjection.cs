using BeHealthy.Frontend.Services;
using BeHealthy.Frontend.Services.Api;
using BeHealthy.Frontend.Services.CurrentUser;
using BeHealthy.Frontend.Services.Interfaces;
using BeHealthy.Frontend.States;
using Microsoft.Extensions.DependencyInjection;

namespace BeHealthy.Frontend;

public static class DependencyInjection
{
    public static IServiceCollection AddFrontendServices(this IServiceCollection services)
    {
        // API client services
        services.AddScoped<IDoctorService, DoctorApiService>();
        services.AddScoped<INurseService, NurseApiService>();
        services.AddScoped<IPatientService, PatientApiService>();
        services.AddScoped<IAppointmentService, AppointmentApiService>();
        services.AddScoped<ISpecialtyService, SpecialtyApiService>();
        services.AddScoped<IDepartmentService, DepartmentApiService>();
        services.AddScoped<IRoomService, RoomApiService>();
        services.AddScoped<IPrescriptionService, PrescriptionApiService>();
        services.AddScoped<IMedicalRecordService, MedicalRecordApiService>();
        services.AddScoped<IAllergyService, AllergyApiService>();
        services.AddScoped<IVisitService, VisitApiService>();
        services.AddScoped<IAppSettingsService, AppSettingsApiService>();
        services.AddScoped<ISeedingService, SeedingApiService>();
        services.AddScoped<IDashboardService, DashboardApiService>();

        // Current-user stub (auth is intentionally out of scope for now)
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        // UI services / state containers
        services.AddScoped<IModalService, ModalService>();
        services.AddScoped<ModalStateService>();
        services.AddScoped<NavMenuState>();
        services.AddScoped<LoaderServiceState>();
        services.AddScoped<BreadcrumbServiceState>();
        services.AddScoped<AlertModalStateService>();
        services.AddScoped<ToastrStateService>();
        services.AddSingleton<ToastService>();

        return services;
    }
}
