namespace BeHealthy.Validation.Common;

/// <summary>
/// Maximum string lengths, shared by the validators and the EF Core configurations.
/// SQLite does not enforce <c>HasMaxLength</c>, so the validators are the only real guard;
/// keeping both on these constants stops them from drifting apart.
/// </summary>
public static class FieldLengths
{
    public const int PersonName = 50;
    public const int Email = 256;
    public const int Gender = 10;
    public const int Address = 200;

    /// <summary>Base64 data URL of an uploaded image (the upload dialog caps files at 1 MB).</summary>
    public const int Image = 1_500_000;

    public const int AllergyName = 128;
    public const int Allergen = 128;
    public const int AllergyNotes = 512;

    public const int AppointmentNotes = 500;

    public const int DepartmentName = 100;
    public const int DepartmentLocation = 100;

    public const int MedicalRecordNotes = 500;

    public const int Medication = 100;
    public const int Dosage = 50;

    public const int RoomName = 100;

    public const int SpecialtyName = 100;

    public const int VisitReason = 256;
    public const int VisitNotes = 1024;

    public const int AppSettingKey = 50;
    public const int AppSettingValue = 500;

    public const int SearchTerm = 100;
    public const int OrderBy = 50;
}
