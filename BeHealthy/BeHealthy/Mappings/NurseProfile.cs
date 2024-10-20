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
            .ForMember(dest => dest.Email,
                opt => opt.MapFrom(src => src.User != null && src.User.Email != null
                                  ? src.User.Email
                                  : string.Empty))
            .ForMember(dest => dest.PhoneNumber,
                opt => opt.MapFrom(src => src.User != null && src.User.PhoneNumber != null
                                  ? src.User.PhoneNumber
                                  : string.Empty));

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
