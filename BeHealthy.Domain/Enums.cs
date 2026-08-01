using BeHealthy.Shared.Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BeHealthy.Domain;

[TypeConverter(typeof(EnumResourceConverter))]
public enum LanguageOptions
{
    English,
    Greek
}

public enum Severity
{
    Info,
    Warning,
    Success,
    Danger
}

public enum MedicalRecordTabs
{ 
    Visits = 0, 
    Allergies = 1, 
    Medications = 2 
}

public enum PatientTabs
{
    GeneralData = 0,
    MedicalRecords = 1,
    Allergies = 2,
    Appointments = 3,
}

public enum DepartmentTabs
{
    GeneralData = 0,
    Doctors = 1,
    Patients = 2,
    Nurses = 3,
    Rooms = 4,
}