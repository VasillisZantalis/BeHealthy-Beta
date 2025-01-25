using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Application.Services;
using Microsoft.Extensions.DependencyInjection;

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
        services.AddScoped<IPrivilegeService, PrivilegeService>();
        services.AddScoped<ISpecialtyService, SpecialtyService>();

        return services;
    }
}
