using AutoMapper;
using BeHealthy.Shared.Models.Dtos.Prescription;
using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Mappings;

public class PrescriptionProfile : Profile
{
    public PrescriptionProfile()
    {
        CreateMap<Prescription, PrescriptionDto>().ReverseMap();
        CreateMap<PrescriptionForCreationDto, Prescription>();
        CreateMap<PrescriptionForUpdateDto, Prescription>().ReverseMap();
    }
}
