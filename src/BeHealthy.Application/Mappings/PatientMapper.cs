namespace BeHealthy.Application.Mappings;

public static class PatientMapper
{
    public static PatientResponse MapToDto(this Patient patient)
    {
        return new PatientResponse
        {
            Id = patient.Id,
            UserId = patient.UserId,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Image = patient.Image,
            PhoneNumber = patient.User?.PhoneNumber ?? string.Empty,
            Email = patient.User?.Email ?? string.Empty,
            CreatedAt = patient.CreatedAt,
            DepartmentId = patient.DepartmentId
        };
    }

    public static Patient MapToDomain(this PatientResponse dto)
    {
        return new Patient
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

    public static Patient MapToDomain(this PatientCreateRequest dto)
    {
        return new Patient
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Image = dto.Image,
            UserId = dto.UserId,
            DepartmentId = dto.DepartmentId,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static Patient MapToDomain(this PatientUpdateRequest dto)
    {
        return new Patient
        {
            Id = dto.Id,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Image = dto.Image,
            UserId = dto.UserId,
            DepartmentId = dto.DepartmentId
        };
    }

    public static PatientUpdateRequest MapToUpdateDto(this Patient patient)
    {
        return new PatientUpdateRequest
        {
            Id = patient.Id,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Image = patient.Image,
            UserId = patient.UserId,
            PhoneNumber = patient.User?.PhoneNumber ?? string.Empty,
            DepartmentId = patient.DepartmentId
        };
    }

    public static PatientUpdateRequest MapToUpdateDto(this PatientResponse dto)
    {
        return new PatientUpdateRequest
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

    public static PatientUpdateRequest MapToDtoForUpdate(this PatientResponse dto)
    {
        return new PatientUpdateRequest
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

    public static PatientResponse MapToDto(this PatientUpdateRequest dto)
    {
        return new PatientResponse
        {
            Id = dto.Id,
            UserId = dto.UserId,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Image = dto.Image,
            PhoneNumber = dto.PhoneNumber,
            CreatedAt = DateTime.UtcNow,
            DepartmentId = dto.DepartmentId
        };
    }

    public static PatientSimpleResponse MapToSimpleDto(this Patient patient)
    {
        return new PatientSimpleResponse
        {
            Id = patient.Id,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            UserId = patient.UserId,
            Image = patient.Image
        };
    }

    public static IEnumerable<PatientResponse> MapToDto(this IEnumerable<Patient> patients)
    {
        return patients.Select(p => p.MapToDto());
    }

    public static ICollection<PatientResponse> MapToDto(this ICollection<Patient> patients)
    {
        return patients.Select(d => d.MapToDto()).ToList();
    }

    public static IEnumerable<Patient> MapToDomain(this IEnumerable<PatientResponse> dtos)
    {
        return dtos.Select(dto => dto.MapToDomain());
    }

    public static IEnumerable<PatientSimpleResponse> MapToSimpleDto(this IEnumerable<Patient> patients)
    {
        return patients.Select(p => p.MapToSimpleDto());
    }
}
