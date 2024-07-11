using AutoMapper;
using BeHealthy.Shared.Models.Dtos.Room;
using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Mappings;

public class RoomProfile : Profile
{
    public RoomProfile()
    {
        CreateMap<Room, RoomDto>().ReverseMap();
        CreateMap<RoomForCreationDto, Room>();
        CreateMap<RoomForUpdateDto, Room>().ReverseMap();
    }
}
