namespace BeHealthy.Application.Mappings;

public static class ProfileMapper
{
    public static DoctorUpdateDto MapToDoctorForUpdateDto(this ProfileDto profile)
    {
        return new DoctorUpdateDto
        {
            Id = profile.Id,
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            Specialty = profile.Specialty ?? string.Empty,
            Image = profile.Image,
            UserId = profile.UserId
        };
    }

    public static PatientUpdateDto MapToPatientForUpdateDto(this ProfileDto profile)
    {
        return new PatientUpdateDto
        {
            Id = profile.Id,
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            Image = profile.Image,
            UserId = profile.UserId
        };
    }

    public static ApplicationUser MapToUserForUpdateDto(this ProfileDto profile, ApplicationUser user)
    {
        user.FirstName = profile.FirstName;
        user.LastName = profile.LastName;

        return user;
    }
}
