namespace BeHealthy.API.Controllers;

/// <summary>Admin-only endpoints backing the seeding modal in the UI.</summary>
[Route("api/[controller]")]
[ApiController]
public class SeedingController(ISeedingService seedingService) : ApiControllerBase
{
    /// <summary>Gets the current row count of every seedable entity.</summary>
    [HttpGet("counts")]
    [ProducesResponseType<Dictionary<string, int>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<Dictionary<string, int>>> GetCounts()
        => Ok(await seedingService.CheckEntityCountsAsync());

    /// <summary>Gets whether the database still needs seeding.</summary>
    [HttpGet("needs-seeding")]
    [ProducesResponseType<bool>(StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> NeedsSeeding()
        => Ok(await seedingService.NeedsSeedingAsync());

    /// <summary>Seeds a number of doctors.</summary>
    [HttpPost("doctors")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SeedDoctors([FromQuery] int count = 1)
    {
        var response = await seedingService.SeedDoctorsAsync(count);
        return response.Success ? Ok() : ProblemFromServiceResponse(response);
    }

    /// <summary>Seeds a number of patients.</summary>
    [HttpPost("patients")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SeedPatients([FromQuery] int count = 1)
    {
        var response = await seedingService.SeedPatientsAsync(count);
        return response.Success ? Ok() : ProblemFromServiceResponse(response);
    }

    /// <summary>Seeds a number of nurses.</summary>
    [HttpPost("nurses")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SeedNurses([FromQuery] int count = 1)
    {
        var response = await seedingService.SeedNursesAsync(count);
        return response.Success ? Ok() : ProblemFromServiceResponse(response);
    }

    /// <summary>Seeds a number of appointments.</summary>
    [HttpPost("appointments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SeedAppointments([FromQuery] int count = 1)
    {
        var response = await seedingService.SeedAppointmentsAsync(count);
        return response.Success ? Ok() : ProblemFromServiceResponse(response);
    }

    /// <summary>Seeds everything selected in <paramref name="options"/> in one call.</summary>
    [HttpPost("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SeedAll(SeedingOptionsDto options)
    {
        var response = await seedingService.SeedAllAsync(options);
        return response.Success ? Ok() : ProblemFromServiceResponse(response);
    }
}
