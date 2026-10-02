using BeHealthy.Shared.Dtos.Appointment;

namespace BeHealthy.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AppointmentsController(IAppointmentService appointmentService) : ApiControllerBase
{
    /// <summary>Gets a paginated, filterable list of appointments.</summary>
    [HttpGet]
    [ProducesResponseType<PaginatedResult<AppointmentResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedResult<AppointmentResponse>>> GetAll([FromQuery] AppointmentQueryParameters parameters, CancellationToken cancellationToken)
        => Ok(await appointmentService.GetAllAppointmentsAsync(parameters, cancellationToken));

    /// <summary>Gets the distribution of appointments by reason, used by the dashboard chart.</summary>
    [HttpGet("reasons")]
    [ProducesResponseType<Dictionary<AppointmentReason, int>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<Dictionary<AppointmentReason, int>>> GetReasonCounts(CancellationToken cancellationToken)
        => Ok(await appointmentService.GetAppointmentReasonCounts(cancellationToken));

    /// <summary>Gets every appointment for a doctor.</summary>
    [HttpGet("by-doctor/{doctorId:int}")]
    [ProducesResponseType<IEnumerable<AppointmentResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AppointmentResponse>>> GetByDoctor(int doctorId, CancellationToken cancellationToken)
        => Ok(await appointmentService.GetAllAppointmentsByDoctorIdAsync(doctorId, cancellationToken));

    /// <summary>Gets every appointment for a patient.</summary>
    [HttpGet("by-patient/{patientId:int}")]
    [ProducesResponseType<IEnumerable<AppointmentResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AppointmentResponse>>> GetByPatient(int patientId, CancellationToken cancellationToken)
        => Ok(await appointmentService.GetAllAppointmentsByPatientIdAsync(patientId, cancellationToken));

    /// <summary>Gets every appointment for a given user (patient or doctor).</summary>
    [HttpGet("by-user/{userId}")]
    [ProducesResponseType<IEnumerable<AppointmentResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AppointmentResponse>>> GetByUser(string userId, CancellationToken cancellationToken)
        => Ok(await appointmentService.GetAllAppointmentsByUserIdAsync(userId, cancellationToken));

    /// <summary>Gets a single appointment by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType<AppointmentResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AppointmentResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var appointment = await appointmentService.GetAppointmentByIdAsync(id, cancellationToken);
        return appointment is null ? NotFoundProblem("Appointment", id) : Ok(appointment);
    }

    /// <summary>Creates a new appointment.</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(AppointmentCreateRequest dto, CancellationToken cancellationToken)
    {
        var response = await appointmentService.AddAppointmentAsync(dto, cancellationToken);
        return response.Success ? StatusCode(StatusCodes.Status201Created) : ProblemFromServiceResponse(response);
    }

    /// <summary>Updates an existing appointment.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, AppointmentUpdateRequest dto, CancellationToken cancellationToken)
    {
        if (EnsureMatchingId(id, dto.Id) is { } mismatch)
            return mismatch;

        var response = await appointmentService.UpdateAppointmentAsync(dto, cancellationToken);
        return response.Success ? NoContent() : ProblemFromServiceResponse(response);
    }

    /// <summary>Deletes an appointment.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await appointmentService.DeleteAppointmentAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpGet("upcoming")]
    public async Task<ActionResult<IEnumerable<AppointmentResponse>>> GetUpcomingAppointments(CancellationToken cancellationToken)
    {
        var appointments = await appointmentService.GetUpcomingAppointmentsAsync(cancellationToken);
        return Ok(appointments);
    }
}
