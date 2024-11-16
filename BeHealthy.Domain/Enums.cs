using System.ComponentModel.DataAnnotations;

namespace BeHealthy.Domain;

public enum UserRole : short
{
    Admin,
    Staff,
    Doctor,
    Nurse,
    Patient
}

public enum SettingType
{
    Checkbox,
    SingleSelect,
    MultiSelect,
    TextField
}

public enum LanguageOptions
{
    English,
    Greek
}

public enum AppointmentStatus
{
    Scheduled,
    Completed,
    Cancelled,
    Rescheduled
}

public enum AppointmentReason
{
    GeneralCheckup,
    FollowUp,
    Illness,
    Injury,
    Prescription
}

public enum Severity
{
    Info,
    Warning,
    Success,
    Danger
}