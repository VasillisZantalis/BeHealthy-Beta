using BeHealthy.Shared.Common;
using System.ComponentModel;

namespace BeHealthy.Frontend.Models;

// UI-oriented enums ported from BeHealthy.Domain so the WASM frontend does not depend on the Domain project.

[TypeConverter(typeof(EnumResourceConverter))]
public enum LanguageOptions
{
    English,
    Greek
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
