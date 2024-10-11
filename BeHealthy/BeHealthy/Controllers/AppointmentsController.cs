using BeHealthy.Services.Interfaces;
using BeHealthy.Shared.Models.Dtos.Appointment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeHealthy.Controllers;

[Route("api/appointments")]
[ApiController]
[Authorize]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService ?? throw new ArgumentNullException(nameof(appointmentService));
    }

    [HttpGet(Name = nameof(GetAllAppointments))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAllAppointments()
    {
        var appointment = await _appointmentService.GetAllAppointmentsAsync();

        return appointment is null ? NotFound() : Ok(appointment);
    }

    [HttpGet("doctor/{doctorId}", Name = nameof(GetAppointmentsByDoctorId))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointmentsByDoctorId(int doctorId)
    {
        var appointment = await _appointmentService.GetAllAppointmentsByDoctorIdAsync(doctorId);
        return appointment is null ? NotFound() : Ok(appointment);
    }

    [HttpGet("patient/{patientId}", Name = nameof(GetAppointmentsByPatientId))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointmentsByPatientId(int patientId)
    {
        var appointment = await _appointmentService.GetAllAppointmentsByPatientIdAsync(patientId);
        return appointment is null ? NotFound() : Ok(appointment);
    }

    [HttpGet("user/{userId}", Name = nameof(GetAppointmentsByUserId))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointmentsByUserId(string userId)
    {
        var appointment = await _appointmentService.GetAllAppointmentsByUserIdAsync(userId);
        return appointment is null ? NotFound() : Ok(appointment);
    }

    [HttpGet("{id:int}", Name = nameof(GetAppointmentById))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AppointmentDto>> GetAppointmentById(int id)
    {
        if (id < 0)
            return BadRequest();

        var appointment = await _appointmentService.GetAppointmentByIdAsync(id);

        return appointment is null ? NotFound() : Ok(appointment);
    }

    [HttpPost(Name = nameof(AddAppointment))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> AddAppointment(AppointmentForCreationDto appointmentDto)
    {
        if (appointmentDto is null)
            return BadRequest();

        if (!ModelState.IsValid)
            return new UnprocessableEntityObjectResult(ModelState);

        await _appointmentService.AddAppointmentAsync(appointmentDto);
        return Created();
    }

    [HttpPut("{id:int}", Name = nameof(UpdateAppointment))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> UpdateAppointment(int id, [FromBody] AppointmentForUpdateDto appointmentDto)
    {
        if (id < 0 || appointmentDto is null)
            return BadRequest();

        if (!ModelState.IsValid)
            return new UnprocessableEntityObjectResult(ModelState);

        await _appointmentService.UpdateAppointmentAsync(appointmentDto);

        return NoContent();
    }

    [HttpDelete("{id:int}", Name = nameof(DeleteAppointment))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteAppointment(int id)
    {
        if (id < 0)
            return BadRequest();

        await _appointmentService.DeleteAppointmentAsync(id);

        return Ok();
    }
}
