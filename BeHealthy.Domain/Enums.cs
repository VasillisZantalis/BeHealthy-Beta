using BeHealthy.Shared.Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BeHealthy.Domain;

[TypeConverter(typeof(EnumResourceConverter))]
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

[TypeConverter(typeof(EnumResourceConverter))]
public enum SettingGroup
{
    Appointment,
    Department,
    Doctor
}

[TypeConverter(typeof(EnumResourceConverter))]
public enum LanguageOptions
{
    English,
    Greek
}

[TypeConverter(typeof(EnumResourceConverter))]
public enum AppointmentStatus
{
    Scheduled,
    Completed,
    Cancelled,
    Rescheduled
}

[TypeConverter(typeof(EnumResourceConverter))]
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

[TypeConverter(typeof(EnumResourceConverter))]
public enum PrivilegeName
{
    EditAppointments,
    DeleteAppointments,
    EditPatient,
    DeletePatient,
    PrescribeMedications,
    ViewPatientPrescriptions,
    GenerateMedicalReports
}

public enum ImportEntity
{
    Patient,
    Doctor,
    Nurse,
    Appointment
}