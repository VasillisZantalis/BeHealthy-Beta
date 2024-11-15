using AutoMapper;
using BeHealthy.Application.Dtos.Room;
using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Mappings;

public class RoomProfile : Profile
{
    public RoomProfile()
    {
        CreateMap<Room, RoomDto>().ReverseMap();
        CreateMap<RoomForCreationDto, Room>();
        CreateMap<RoomForUpdateDto, Room>().ReverseMap();
    }
}
