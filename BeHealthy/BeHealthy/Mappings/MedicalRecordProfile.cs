using AutoMapper;
using BeHealthy.Shared.Models.Dtos.MedicalRecord;
using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Mappings;

public class MedicalRecordProfile : Profile
{
    public MedicalRecordProfile()
    {
        CreateMap<MedicalRecord, MedicalRecordDto>().ReverseMap();
        CreateMap<MedicalRecordForCreationDto, MedicalRecord>();
        CreateMap<MedicalRecordForUpdateDto, MedicalRecord>().ReverseMap();
    }
}
