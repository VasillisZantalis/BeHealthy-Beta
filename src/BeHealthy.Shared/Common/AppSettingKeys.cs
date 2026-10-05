namespace BeHealthy.Shared.Common;

/// <summary>
/// Keys of the rows in the AppSettings table. Use these instead of string literals so the API,
/// the front end and the seed data cannot drift apart.
/// </summary>
public static class AppSettingKeys
{
    public const string AppointmentRequiresRoom = "AppointmentRequiresRoom";
    public const string NurseIsRequiredForAppointment = "NurseIsRequiredForAppointment";
    public const string DoNotAllowDoctorWithoutSpecialty = "DoNotAllowDoctorWithoutSpecialty";
    public const string DepartmentRequiresSupervisor = "DepartmentRequiresSupervisor";
    public const string DefaultDepartmentSupervison = "DefaultDepartmentSupervison";
}
