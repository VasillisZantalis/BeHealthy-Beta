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

public enum ColorOptions
{
    Red,
    Purple,
    Blue,
    Green,
    Black,
    Yellow,
}