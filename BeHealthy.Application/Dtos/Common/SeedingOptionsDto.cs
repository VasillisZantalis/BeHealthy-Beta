namespace BeHealthy.Application.Dtos.Common;

public class SeedingOptionsDto
{
    public bool SeedDoctors { get; set; }
    public int DoctorCount { get; set; } = 1;

    public bool SeedPatients { get; set; }
    public int PatientCount { get; set; } = 1;

    public bool SeedNurses { get; set; }
    public int NurseCount { get; set; } = 1;

    public bool SeedAppointments { get; set; }
    public int AppointmentCount { get; set; } = 1;
}