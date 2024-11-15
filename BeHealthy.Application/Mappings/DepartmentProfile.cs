using AutoMapper;
using BeHealthy.Application.Dtos.Department;
using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Mappings;

public class DepartmentProfile : Profile
{
    public DepartmentProfile()
    {
        CreateMap<Department, DepartmentDto>().ReverseMap();
        CreateMap<DepartmentForCreationDto, Department>();
        CreateMap<DepartmentForUpdateDto, Department>().ReverseMap();
    }
}
