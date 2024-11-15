using AutoMapper;
using BeHealthy.Application.Dtos.Appointment;
using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Mappings;

public class AppointmentProfile : Profile
{
    public AppointmentProfile()
    {
        CreateMap<Appointment, AppointmentDto>().ReverseMap();
        CreateMap<AppointmentForCreationDto, Appointment>();
        CreateMap<AppointmentForUpdateDto, Appointment>().ReverseMap();
    }
}
