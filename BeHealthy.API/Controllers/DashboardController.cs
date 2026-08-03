using BeHealthy.Shared.Dtos.Dashboard;

namespace BeHealthy.API.Controllers;

/// <summary>Aggregates data for the dashboard widgets in a single round trip.</summary>
[Route("api/[controller]")]
[ApiController]
public class DashboardController(
    IPatientService patientService,
    IDoctorService doctorService,
    INurseService nurseService,
    IAppointmentService appointmentService,
    IUserService userService) : ApiControllerBase
{
    /// <summary>Gets the dashboard summary: entity counts, appointment reason distribution, and users per role.</summary>
    [HttpGet("summary")]
    [ProducesResponseType<DashboardSummaryResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<DashboardSummaryResponse>> GetSummary()
    {
        var summary = new DashboardSummaryResponse
        {
            PatientCount = await patientService.GetPatientCountAsync(),
            DoctorCount = await doctorService.GetDoctorCountAsync(),
            NurseCount = await nurseService.GetNurseCountAsync(),
            AppointmentReasonCounts = await appointmentService.GetAppointmentReasonCounts(),
            UsersInRolesCount = await userService.GetUsersInRolesCount()
        };

        return Ok(summary);
    }
}
