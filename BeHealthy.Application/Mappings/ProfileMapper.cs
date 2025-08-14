namespace BeHealthy.Application.Mappings;

public static class ProfileMapper
{
    public static DoctorForUpdateDto MapToDoctorForUpdateDto(this ProfileDto profile)
    {
        return new DoctorForUpdateDto
        {
            Id = profile.Id,
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            Specialty = profile.Specialty ?? string.Empty,
            Image = profile.Image,
            UserId = profile.UserId
        };
    }
}
