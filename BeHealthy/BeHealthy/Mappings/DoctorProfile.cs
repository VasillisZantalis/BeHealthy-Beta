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
                opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Email,
                opt => opt.MapFrom(src => src.User != null && src.User.Email != null
                                  ? src.User.Email
                                  : string.Empty))
            .ForMember(dest => dest.PhoneNumber,
                opt => opt.MapFrom(src => src.User != null && src.User.PhoneNumber != null
                                  ? src.User.PhoneNumber
                                  : string.Empty));

        CreateMap<DoctorForCreationDto, Doctor>();

        CreateMap<DoctorForUpdateDto, Doctor>()
            .ReverseMap();
    }
}
