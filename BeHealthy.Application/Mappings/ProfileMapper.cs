namespace BeHealthy.Application.Mappings;

public static class ProfileMapper
{
    public static DoctorUpdateRequest MapToDoctorForUpdateDto(this ProfileResponse profile)
    {
        return new DoctorUpdateRequest
        {
            Id = profile.Id,
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            Specialty = profile.Specialty ?? string.Empty,
            Image = profile.Image,
            UserId = profile.UserId
        };
    }

    public static PatientUpdateRequest MapToPatientForUpdateDto(this ProfileResponse profile)
    {
        return new PatientUpdateRequest
        {
            Id = profile.Id,
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            Image = profile.Image,
            UserId = profile.UserId
        };
    }

    public static NurseUpdateRequest MapToNurseForUpdateDto(this ProfileResponse profile)
    {
        return new NurseUpdateRequest
        {
            Id = profile.Id,
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            Image = profile.Image,
            UserId = profile.UserId
        };
    }

    public static ApplicationUser MapToUserForUpdateDto(this ProfileResponse profile, ApplicationUser user)
    {
        user.FirstName = profile.FirstName;
        user.LastName = profile.LastName;

        return user;
    }
}
