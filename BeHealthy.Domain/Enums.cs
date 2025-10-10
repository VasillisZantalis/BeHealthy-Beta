using BeHealthy.Shared.Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BeHealthy.Domain;

[TypeConverter(typeof(EnumResourceConverter))]
public enum UserRole : short
{
    Admin = 0,
    Staff = 1,
    Doctor = 2,
    Nurse = 3,
    Patient = 4
}

public enum SettingType
{
    Checkbox = 0,
    SingleSelect = 1,
    MultiSelect = 2,
    TextField = 3
}

[TypeConverter(typeof(EnumResourceConverter))]
public enum SettingGroup
{
    Appointment = 0,
    Department = 1,
    Doctor = 2
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
    Scheduled = 0,
    Completed = 1,
    Cancelled = 2,
    Rescheduled = 3
}

[TypeConverter(typeof(EnumResourceConverter))]
public enum AppointmentReason
{
    GeneralCheckup = 0,
    FollowUp = 1,
    Illness = 2,
    Injury = 3,
    Prescription = 4
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
    EditAppointments = 0,
    DeleteAppointments = 1,
    EditPatient = 2,
    DeletePatient = 3,
    PrescribeMedications = 4,
    ViewPatientPrescriptions = 5,
    GenerateMedicalReports = 6
}

public enum AllergySeverity
{
    Mild = 0,
    Moderate = 1,
    Severe = 2
}

public enum PatientTabs 
{ 
    Visits = 0, 
    Allergies = 1, 
    Medications = 2 
}

public enum DepartmentTabs
{
    GeneralData = 0,
    Doctors = 1,
    Patients = 2,
    Nurses = 3,
    Rooms = 4,
}