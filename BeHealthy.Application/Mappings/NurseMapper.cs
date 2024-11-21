using BeHealthy.Application.Dtos.Nurse;
using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Mappings
{
    public static class NurseMapper
    {
        public static NurseDto MapToDto(this Nurse nurse)
        {
            return new NurseDto
            {
                Id = nurse.Id,
                UserId = nurse.UserId,
                FirstName = nurse.FirstName,
                LastName = nurse.LastName,
                Image = nurse.Image,
                PhoneNumber = nurse.User?.PhoneNumber ?? string.Empty,
                Email = nurse.User?.Email ?? string.Empty,
                CreatedAt = nurse.CreatedAt,
                DepartmentId = nurse.DepartmentId
            };
        }

        public static Nurse MapToDomain(this NurseDto dto)
        {
            return new Nurse
            {
                Id = dto.Id,
                UserId = dto.UserId ?? string.Empty,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Image = dto.Image,
                CreatedAt = dto.CreatedAt,
                DepartmentId = dto.DepartmentId
            };
        }

        public static Nurse MapToDomain(this NurseForCreationDto dto)
        {
            return new Nurse
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Image = dto.Image,
                UserId = dto.UserId,
                DepartmentId = dto.DepartmentId
            };
        }

        public static Nurse MapToDomain(this NurseForUpdateDto dto)
        {
            return new Nurse
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Image = dto.Image,
                UserId = dto.UserId,
                DepartmentId = dto.DepartmentId
            };
        }

        public static NurseForUpdateDto MapToUpdateDto(this NurseDto dto)
        {
            return new NurseForUpdateDto
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Image = dto.Image,
                UserId = dto.UserId ?? string.Empty,
                PhoneNumber = dto.PhoneNumber,
                DepartmentId = dto.DepartmentId
            };
        }

        public static IEnumerable<NurseDto> MapToDto(this IEnumerable<Nurse> nurses)
        {
            return nurses.Select(n => n.MapToDto());
        }

        public static IEnumerable<Nurse> MapToDomain(this IEnumerable<NurseDto> dtos)
        {
            return dtos.Select(dto => dto.MapToDomain());
        }
    }
}
