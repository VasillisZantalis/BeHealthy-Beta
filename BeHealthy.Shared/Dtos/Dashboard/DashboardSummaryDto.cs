namespace BeHealthy.Shared.Dtos.Dashboard;

public class DashboardSummaryDto
{
    public int PatientCount { get; set; }
    public int DoctorCount { get; set; }
    public int NurseCount { get; set; }
    public Dictionary<AppointmentReason, int> AppointmentReasonCounts { get; set; } = [];
    public Dictionary<string, int> UsersInRolesCount { get; set; } = [];
}
