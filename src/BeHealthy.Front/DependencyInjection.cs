using BeHealthy.Front.Services;
using BeHealthy.Front.Services.Api;
using BeHealthy.Front.Services.CurrentUser;
using BeHealthy.Front.Services.Interfaces;
using BeHealthy.Front.Services.Validation;
using BeHealthy.Front.States;
using BeHealthy.Validation;
using BeHealthy.Validation.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace BeHealthy.Front;

public static class DependencyInjection
{
    public static IServiceCollection AddFrontServices(this IServiceCollection services)
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

        services.AddScoped<ICurrentUserService, CurrentUserService>();

        // The same validators the API runs (BeHealthy.Validation), resolved from DI by Blazored's
        // <FluentValidationValidator /> and by pages that inject IValidator<TRequest>.
        services.AddSharedValidators();
        services.AddScoped<IValidationSettingsProvider, ValidationSettingsProvider>();

        // UI services / state containers
        services.AddScoped<IModalService, ModalService>();
        services.AddScoped<ModalStateService>();
        services.AddScoped<NavMenuState>();
        services.AddScoped<LoaderServiceState>();
        services.AddScoped<BreadcrumbServiceState>();
        services.AddScoped<AlertModalStateService>();
        services.AddScoped<ToastrStateService>();
        services.AddScoped<ToastService>();

        return services;
    }
}
