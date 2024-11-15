using AutoMapper;
using BeHealthy.Application.Dtos.Prescription;
using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Mappings;

public class PrescriptionProfile : Profile
{
    public PrescriptionProfile()
    {
        CreateMap<Prescription, PrescriptionDto>().ReverseMap();
        CreateMap<PrescriptionForCreationDto, Prescription>();
        CreateMap<PrescriptionForUpdateDto, Prescription>().ReverseMap();
    }
}
