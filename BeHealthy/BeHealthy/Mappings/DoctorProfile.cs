using AutoMapper;
using BeHealthy.Shared.Models.Dtos.Doctor;
using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Mappings;

public class DoctorProfile : Profile
{
    public DoctorProfile()
    {
        CreateMap<Doctor, DoctorDto>()
            .ForMember(dest => dest.UserId, 
                opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.FirstName, 
                opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, 
                opt => opt.MapFrom(src => src.LastName));

        CreateMap<DoctorForCreationDto, Doctor>();
            //.ForMember(dest => dest.User.FirstName,
            //    opt => opt.MapFrom(src => src.FirstName))
            //.ForMember(dest => dest.User.LastName,
            //    opt => opt.MapFrom(src => src.LastName));

        CreateMap<DoctorForUpdateDto, Doctor>()
            //.ForMember(dest => dest.User.FirstName,
            //    opt => opt.MapFrom(src => src.FirstName))
            //.ForMember(dest => dest.User.LastName,
            //    opt => opt.MapFrom(src => src.LastName))
            .ReverseMap();
    }
}
