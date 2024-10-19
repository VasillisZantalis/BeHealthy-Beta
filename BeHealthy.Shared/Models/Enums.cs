using System.ComponentModel.DataAnnotations;

namespace BeHealthy.Shared.Models;

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
    Spanish,
    French
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
    [Display(Name = "General Check-up")]
    GeneralCheckup,

    [Display(Name = "Follow Up")]
    FollowUp,

    [Display(Name = "Illness")]
    Illness,

    [Display(Name = "Injury")]
    Injury,

    [Display(Name = "Prescription")]
    Prescription
}