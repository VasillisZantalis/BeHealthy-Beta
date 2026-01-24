namespace BeHealthy.Application.Mappings;

public static class PatientMapper
{
    public static PatientDto MapToDto(this Patient patient)
    {
        return new PatientDto
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

    public static Patient MapToDomain(this PatientDto dto)
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

    public static Patient MapToDomain(this PatientCreateDto dto)
    {
        return new Patient
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Image = dto.Image,
            UserId = dto.UserId,
            DepartmentId = dto.DepartmentId
        };
    }

    public static Patient MapToDomain(this PatientUpdateDto dto)
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

    public static PatientUpdateDto MapToUpdateDto(this Patient patient)
    {
        return new PatientUpdateDto
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

    public static PatientUpdateDto MapToUpdateDto(this PatientDto dto)
    {
        return new PatientUpdateDto
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

    public static PatientUpdateDto MapToDtoForUpdate(this PatientDto dto)
    {
        return new PatientUpdateDto
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

    public static PatientDto MapToDto(this PatientUpdateDto dto)
    {
        return new PatientDto
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

    public static PatientSimpleDto MapToSimpleDto(this Patient patient)
    {
        return new PatientSimpleDto
        {
            Id = patient.Id,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            UserId = patient.UserId,
            Image = patient.Image
        };
    }

    public static IEnumerable<PatientDto> MapToDto(this IEnumerable<Patient> patients)
    {
        return patients.Select(p => p.MapToDto());
    }

    public static ICollection<PatientDto> MapToDto(this ICollection<Patient> patients)
    {
        return patients.Select(d => d.MapToDto()).ToList();
    }

    public static IEnumerable<Patient> MapToDomain(this IEnumerable<PatientDto> dtos)
    {
        return dtos.Select(dto => dto.MapToDomain());
    }

    public static IEnumerable<PatientSimpleDto> MapToSimpleDto(this IEnumerable<Patient> patients)
    {
        return patients.Select(p => p.MapToSimpleDto());
    }
}
