using AutoMapper;
using BeHealthy.Application.Dtos.MedicalRecord;
using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Mappings;

public class MedicalRecordProfile : Profile
{
    public MedicalRecordProfile()
    {
        CreateMap<MedicalRecord, MedicalRecordDto>().ReverseMap();
        CreateMap<MedicalRecordForCreationDto, MedicalRecord>();
        CreateMap<MedicalRecordForUpdateDto, MedicalRecord>().ReverseMap();
    }
}
