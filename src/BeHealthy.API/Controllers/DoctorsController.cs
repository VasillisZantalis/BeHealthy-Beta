using BeHealthy.Shared.Dtos.Appointment;
using BeHealthy.Shared.Dtos.Doctor;

namespace BeHealthy.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DoctorsController(IDoctorService doctorService, IAppointmentService appointmentService) : ApiControllerBase
{
    /// <summary>Gets a paginated, filterable list of doctors.</summary>
    [HttpGet]
    [Authorize(Roles = RoleGroups.AllUsers)]
    [ProducesResponseType<PaginatedResult<DoctorResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedResult<DoctorResponse>>> GetAll([FromQuery] DoctorQueryParameters parameters, CancellationToken cancellationToken)
        => Ok(await doctorService.GetAllDoctorsAsync(parameters, cancellationToken));

    /// <summary>Gets a lightweight list of doctors for dropdowns.</summary>
    [HttpGet("simple")]
    [Authorize(Roles = RoleGroups.AllUsers)]
    [ProducesResponseType<IEnumerable<DoctorSimpleResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DoctorSimpleResponse>>> GetAllSimple(CancellationToken cancellationToken)
        => Ok(await doctorService.GetAllDoctorsSimpleAsync(cancellationToken));

    /// <summary>Gets the total number of doctors.</summary>
    [HttpGet("count")]
    [Authorize(Roles = RoleGroups.AllUsers)]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> GetCount(CancellationToken cancellationToken)
        => Ok(await doctorService.GetDoctorCountAsync(cancellationToken));

    /// <summary>Gets a single doctor by id.</summary>
    [HttpGet("{id:int}")]
    [Authorize(Roles = RoleGroups.AllUsers)]
    [ProducesResponseType<DoctorResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DoctorResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var doctor = await doctorService.GetDoctorByIdAsync(id, cancellationToken);
        return doctor is null ? NotFoundProblem("Doctor", id) : Ok(doctor);
    }

    /// <summary>Gets every appointment for a doctor.</summary>
    [HttpGet("{id:int}/appointments")]
    [Authorize(Roles = RoleGroups.AllUsers)]
    [ProducesResponseType<IEnumerable<AppointmentResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AppointmentResponse>>> GetAppointments(int id, CancellationToken cancellationToken)
        => Ok(await appointmentService.GetAllAppointmentsByDoctorIdAsync(id, cancellationToken));

    /// <summary>Creates a new doctor and their user account.</summary>
    [HttpPost]
    [Authorize(Roles = RoleGroups.Admin)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(DoctorCreateRequest dto, CancellationToken cancellationToken)
    {
        var response = await doctorService.AddDoctorAsync(dto, cancellationToken);
        return response.Success ? StatusCode(StatusCodes.Status201Created) : ProblemFromServiceResponse(response);
    }

    /// <summary>Updates an existing doctor.</summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = RoleGroups.Admin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, DoctorUpdateRequest dto, CancellationToken cancellationToken)
    {
        if (EnsureMatchingId(id, dto.Id) is { } mismatch)
        {
            return mismatch;
        }

        var response = await doctorService.UpdateDoctorAsync(dto, cancellationToken);
        return response.Success ? NoContent() : ProblemFromServiceResponse(response);
    }

    /// <summary>Deletes a doctor.</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = RoleGroups.Admin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await doctorService.DeleteDoctorAsync(id, cancellationToken);
        return NoContent();
    }
}
