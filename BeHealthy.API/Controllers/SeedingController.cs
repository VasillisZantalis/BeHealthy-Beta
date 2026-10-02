namespace BeHealthy.API.Controllers;

/// <summary>Admin-only endpoints backing the seeding modal in the UI.</summary>
[Route("api/[controller]")]
[ApiController]
public class SeedingController(ISeedingService seedingService) : ApiControllerBase
{
    /// <summary>Gets the current row count of every seedable entity.</summary>
    [HttpGet("counts")]
    [Authorize(Roles = RoleGroups.Admin)]
    [ProducesResponseType<Dictionary<string, int>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<Dictionary<string, int>>> GetCounts(CancellationToken cancellationToken)
        => Ok(await seedingService.CheckEntityCountsAsync(cancellationToken));

    /// <summary>Gets whether the database still needs seeding.</summary>
    [HttpGet("needs-seeding")]
    [Authorize(Roles = RoleGroups.Admin)]
    [ProducesResponseType<bool>(StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> NeedsSeeding(CancellationToken cancellationToken)
        => Ok(await seedingService.NeedsSeedingAsync(cancellationToken));

    /// <summary>Seeds a number of doctors.</summary>
    [HttpPost("doctors")]
    [Authorize(Roles = RoleGroups.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SeedDoctors([FromQuery] int count = 1, CancellationToken cancellationToken = default)
    {
        var response = await seedingService.SeedDoctorsAsync(count, cancellationToken);
        return response.Success ? Ok() : ProblemFromServiceResponse(response);
    }

    /// <summary>Seeds a number of patients.</summary>
    [HttpPost("patients")]
    [Authorize(Roles = RoleGroups.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SeedPatients([FromQuery] int count = 1, CancellationToken cancellationToken = default)
    {
        var response = await seedingService.SeedPatientsAsync(count, cancellationToken);
        return response.Success ? Ok() : ProblemFromServiceResponse(response);
    }

    /// <summary>Seeds a number of nurses.</summary>
    [HttpPost("nurses")]
    [Authorize(Roles = RoleGroups.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SeedNurses([FromQuery] int count = 1, CancellationToken cancellationToken = default)
    {
        var response = await seedingService.SeedNursesAsync(count, cancellationToken);
        return response.Success ? Ok() : ProblemFromServiceResponse(response);
    }

    /// <summary>Seeds a number of appointments.</summary>
    [HttpPost("appointments")]
    [Authorize(Roles = RoleGroups.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SeedAppointments([FromQuery] int count = 1, CancellationToken cancellationToken = default)
    {
        var response = await seedingService.SeedAppointmentsAsync(count, cancellationToken);
        return response.Success ? Ok() : ProblemFromServiceResponse(response);
    }

    /// <summary>Seeds everything selected in <paramref name="options"/> in one call.</summary>
    [HttpPost("all")]
    [Authorize(Roles = RoleGroups.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SeedAll(SeedingOptionsRequest options, CancellationToken cancellationToken)
    {
        var response = await seedingService.SeedAllAsync(options, cancellationToken);
        return response.Success ? Ok() : ProblemFromServiceResponse(response);
    }
}
