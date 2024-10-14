using BeHealthy.Client.Services.Interfaces;
using BeHealthy.Client.Services;

namespace BeHealthy.Client.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddClientServices(this IServiceCollection services)
    {
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<INurseService, NurseService>();
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IRoomService, RoomService>();

        return services;
    }
}
