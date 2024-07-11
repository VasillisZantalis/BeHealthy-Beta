using AutoMapper;
using BeHealthy.Shared.Models.Dtos.Department;
using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Mappings;

public class DepartmentProfile : Profile
{
    public DepartmentProfile()
    {
        CreateMap<Department, DepartmentDto>().ReverseMap();
        CreateMap<DepartmentForCreationDto, Department>();
        CreateMap<DepartmentForUpdateDto, Department>().ReverseMap();
    }
}
