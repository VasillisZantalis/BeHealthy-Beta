namespace BeHealthy.API.Authorization;

/// <summary>
/// Comma-separated role lists for <see cref="AuthorizeAttribute.Roles"/>, built from <see cref="UserRole"/>
/// so a renamed role breaks the build instead of silently locking users out.
/// </summary>
public static class RoleGroups
{
    private const string Separator = ",";

    public const string Admin = nameof(UserRole.Admin);

    public const string Doctor = nameof(UserRole.Doctor);

    public const string Patient = nameof(UserRole.Patient);

    /// <summary>Users who have their own profile and take part in appointments.</summary>
    public const string AppointmentParticipants = Doctor + Separator + nameof(UserRole.Nurse) + Separator + Patient;

    /// <summary>Users who manage the hospital's structure and registrations.</summary>
    public const string Administration = Admin + Separator + nameof(UserRole.Staff);

    /// <summary>Users allowed to prescribe medication.</summary>
    public const string Prescribers = Admin + Separator + nameof(UserRole.Doctor);

    /// <summary>Users who maintain clinical data (visits, medical records, allergies).</summary>
    public const string Clinicians = Prescribers + Separator + nameof(UserRole.Nurse);

    /// <summary>Every employee of the hospital.</summary>
    public const string MedicalStaff = Clinicians + Separator + nameof(UserRole.Staff);

    /// <summary>Every role, patients included.</summary>
    public const string AllUsers = MedicalStaff + Separator + nameof(UserRole.Patient);
}
