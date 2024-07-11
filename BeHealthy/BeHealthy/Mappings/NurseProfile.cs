using AutoMapper;
using BeHealthy.Shared.Models.Dtos.Doctor;
using BeHealthy.Shared.Models.Dtos.Nurse;
using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Mappings;

public class NurseProfile : Profile
{
    public NurseProfile()
    {
        CreateMap<Nurse, NurseDto>()
            //.ForMember(dest => dest.UserId,
            //    opt => opt.MapFrom(src => src.User.Id))
            //.ForMember(dest => dest.FirstName,
            //    opt => opt.MapFrom(src => src.User.FirstName))
            //.ForMember(dest => dest.LastName,
            //    opt => opt.MapFrom(src => src.User.LastName))
            .ReverseMap();

        CreateMap<NurseForCreationDto, Nurse>();
            //.ForMember(dest => dest.User.FirstName,
            //    opt => opt.MapFrom(src => src.FirstName))
            //.ForMember(dest => dest.User.LastName,
            //    opt => opt.MapFrom(src => src.LastName));

        CreateMap<NurseForUpdateDto, Nurse>()
            //.ForMember(dest => dest.User.FirstName,
            //    opt => opt.MapFrom(src => src.FirstName))
            //.ForMember(dest => dest.User.LastName,
            //    opt => opt.MapFrom(src => src.LastName))
            .ReverseMap();
    }
}
