using AutoMapper;
using BeHealthy.Shared.Models.Dtos.Patient;
using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Mappings;

public class PatientProfile : Profile
{
    public PatientProfile()
    {
        CreateMap<Patient, PatientDto>()
            .ForMember(dest => dest.Email,
                opt => opt.MapFrom(src => src.User != null && src.User.Email != null
                                  ? src.User.Email
                                  : string.Empty))
            .ForMember(dest => dest.PhoneNumber,
                opt => opt.MapFrom(src => src.User != null && src.User.PhoneNumber != null
                                  ? src.User.PhoneNumber
                                  : string.Empty));

        CreateMap<PatientForCreationDto, Patient>();
            //.ForMember(dest => dest.User.FirstName,
            //    opt => opt.MapFrom(src => src.FirstName))
            //.ForMember(dest => dest.User.LastName,
            //    opt => opt.MapFrom(src => src.LastName));

        CreateMap<PatientForUpdateDto, Patient>()
            //.ForMember(dest => dest.User.FirstName,
            //    opt => opt.MapFrom(src => src.FirstName))
            //.ForMember(dest => dest.User.LastName,
            //    opt => opt.MapFrom(src => src.LastName))
            .ReverseMap();
    }
}
