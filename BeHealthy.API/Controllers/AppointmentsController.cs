using BeHealthy.Shared.Dtos.Appointment;

namespace BeHealthy.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AppointmentsController(IAppointmentService appointmentService) : ApiControllerBase
{
    /// <summary>Gets a paginated, filterable list of appointments.</summary>
    [HttpGet]
    [ProducesResponseType<PaginatedResult<AppointmentResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedResult<AppointmentResponse>>> GetAll([FromQuery] AppointmentQueryParameters parameters)
        => Ok(await appointmentService.GetAllAppointmentsAsync(parameters));

    /// <summary>Gets the distribution of appointments by reason, used by the dashboard chart.</summary>
    [HttpGet("reasons")]
    [ProducesResponseType<Dictionary<AppointmentReason, int>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<Dictionary<AppointmentReason, int>>> GetReasonCounts()
        => Ok(await appointmentService.GetAppointmentReasonCounts());

    /// <summary>Gets every appointment for a doctor.</summary>
    [HttpGet("by-doctor/{doctorId:int}")]
    [ProducesResponseType<IEnumerable<AppointmentResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AppointmentResponse>>> GetByDoctor(int doctorId)
        => Ok(await appointmentService.GetAllAppointmentsByDoctorIdAsync(doctorId));

    /// <summary>Gets every appointment for a patient.</summary>
    [HttpGet("by-patient/{patientId:int}")]
    [ProducesResponseType<IEnumerable<AppointmentResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AppointmentResponse>>> GetByPatient(int patientId)
        => Ok(await appointmentService.GetAllAppointmentsByPatientIdAsync(patientId));

    /// <summary>Gets every appointment for a given user (patient or doctor).</summary>
    [HttpGet("by-user/{userId}")]
    [ProducesResponseType<IEnumerable<AppointmentResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AppointmentResponse>>> GetByUser(string userId)
        => Ok(await appointmentService.GetAllAppointmentsByUserIdAsync(userId));

    /// <summary>Gets a single appointment by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType<AppointmentResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AppointmentResponse>> GetById(int id)
    {
        var appointment = await appointmentService.GetAppointmentByIdAsync(id);
        return appointment is null ? NotFoundProblem("Appointment", id) : Ok(appointment);
    }

    /// <summary>Creates a new appointment.</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(AppointmentCreateRequest dto)
    {
        var response = await appointmentService.AddAppointmentAsync(dto);
        return response.Success ? StatusCode(StatusCodes.Status201Created) : ProblemFromServiceResponse(response);
    }

    /// <summary>Updates an existing appointment.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, AppointmentUpdateRequest dto)
    {
        if (EnsureMatchingId(id, dto.Id) is { } mismatch)
            return mismatch;

        var response = await appointmentService.UpdateAppointmentAsync(dto);
        return response.Success ? NoContent() : ProblemFromServiceResponse(response);
    }

    /// <summary>Deletes an appointment.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id)
    {
        await appointmentService.DeleteAppointmentAsync(id);
        return NoContent();
    }
}
