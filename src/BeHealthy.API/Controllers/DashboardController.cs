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
    [Authorize(Roles = RoleGroups.AllUsers)]
    [ProducesResponseType<DashboardSummaryResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<DashboardSummaryResponse>> GetSummary(CancellationToken cancellationToken)
    {
        var summary = new DashboardSummaryResponse
        {
            PatientCount = await patientService.GetPatientCountAsync(cancellationToken),
            DoctorCount = await doctorService.GetDoctorCountAsync(cancellationToken),
            NurseCount = await nurseService.GetNurseCountAsync(cancellationToken),
            AppointmentReasonCounts = await appointmentService.GetAppointmentReasonCounts(cancellationToken),
            UsersInRolesCount = await userService.GetUsersInRolesCount(cancellationToken)
        };

        return Ok(summary);
    }
}
