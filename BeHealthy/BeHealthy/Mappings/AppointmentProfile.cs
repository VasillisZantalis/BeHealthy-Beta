using AutoMapper;
using BeHealthy.Shared.Models.Dtos.Appointment;
using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Mappings;

public class AppointmentProfile : Profile
{
    public AppointmentProfile()
    {
        CreateMap<Appointment, AppointmentDto>().ReverseMap();
        CreateMap<AppointmentForCreationDto, Appointment>();
        CreateMap<AppointmentForUpdateDto, Appointment>().ReverseMap();
    }
}
