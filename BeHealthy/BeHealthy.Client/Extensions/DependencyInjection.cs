using BeHealthy.Client.Services;
using BeHealthy.Shared.Interfaces;

namespace BeHealthy.Client.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddClientServices(this IServiceCollection services)
    {
        services.AddScoped<IAppointmentService, AppointmentClientService>();
        services.AddScoped<IUserService, UserClientService>();
        services.AddScoped<IDoctorService, DoctorClientService>();
        services.AddScoped<INurseService, NurseClientService>();
        services.AddScoped<IPatientService, PatientClientService>();
        services.AddScoped<IRoomService, RoomClientService>();

        return services;
    }
}
