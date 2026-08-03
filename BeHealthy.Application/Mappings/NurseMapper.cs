using BeHealthy.Shared.Dtos.Nurse;
using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Mappings
{
    public static class NurseMapper
    {
        public static NurseResponse MapToDto(this Nurse nurse)
        {
            return new NurseResponse
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

        public static Nurse MapToDomain(this NurseResponse dto)
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

        public static Nurse MapToDomain(this NurseCreateRequest dto)
        {
            return new Nurse
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Image = dto.Image,
                UserId = dto.UserId,
                DepartmentId = dto.DepartmentId,
                CreatedAt = DateTime.UtcNow
            };
        }

        public static Nurse MapToDomain(this NurseUpdateRequest dto)
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

        public static NurseUpdateRequest MapToUpdateDto(this NurseResponse dto)
        {
            return new NurseUpdateRequest
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

        public static NurseSimpleResponse MapToSimpleDto(this Nurse nurse)
        {
            return new NurseSimpleResponse
            {
                Id = nurse.Id,
                FirstName = nurse.FirstName,
                LastName = nurse.LastName,
                UserId = nurse.UserId,
                Image = nurse.Image
            };
        }

        public static IEnumerable<NurseResponse> MapToDto(this IEnumerable<Nurse> nurses)
        {
            return nurses.Select(n => n.MapToDto());
        }

        public static ICollection<NurseResponse> MapToDto(this ICollection<Nurse> nurses)
        {
            return nurses.Select(d => d.MapToDto()).ToList();
        }

        public static IEnumerable<Nurse> MapToDomain(this IEnumerable<NurseResponse> dtos)
        {
            return dtos.Select(dto => dto.MapToDomain());
        }
    }
}
