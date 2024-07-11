using BeHealthy.Services.Interfaces;
using BeHealthy.Shared.Models.Dtos.Patient;
using Microsoft.AspNetCore.Mvc;

namespace BeHealthy.Controllers;

[Route("api/patients")]
[ApiController]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService ?? throw new ArgumentNullException(nameof(patientService));
    }

    [HttpGet(Name = nameof(GetAllPatients))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<PatientDto>>> GetAllPatients()
    {
        var patients = await _patientService.GetAllPatientsAsync();

        return patients is null ? NotFound() : Ok(patients);
    }

    [HttpGet("{id:int}", Name = nameof(GetPatientById))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PatientDto>> GetPatientById(int id)
    {
        if (id < 0)
            return BadRequest();

        var patient = await _patientService.GetPatientByIdAsync(id);

        return patient is null ? NotFound() : Ok(patient);
    }

    [HttpPost(Name = nameof(AddPatient))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> AddPatient(PatientForCreationDto patientDto)
    {
        if (patientDto is null)
            return BadRequest();

        if (!ModelState.IsValid)
            return new UnprocessableEntityObjectResult(ModelState);

        await _patientService.AddPatientAsync(patientDto);
        return Created();
    }

    [HttpPut("{id:int}", Name = nameof(UpdatePatient))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> UpdatePatient(int id, [FromBody] PatientForUpdateDto patientDto)
    {
        if (id < 0 || patientDto is null)
            return BadRequest();

        if (!ModelState.IsValid)
            return new UnprocessableEntityObjectResult(ModelState);

        await _patientService.UpdatePatientAsync(patientDto);

        return NoContent();
    }

    [HttpDelete("{id:int}", Name = nameof(DeletePatient))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeletePatient(int id)
    {
        if (id < 0)
            return BadRequest();

        await _patientService.DeletePatientAsync(id);

        return Ok();
    }
}
