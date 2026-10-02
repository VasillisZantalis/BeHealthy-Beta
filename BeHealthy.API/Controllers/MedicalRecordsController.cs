using BeHealthy.Shared.Dtos.MedicalRecord;

namespace BeHealthy.API.Controllers;

[Route("api/medical-records")]
[ApiController]
public class MedicalRecordsController(IMedicalRecordService medicalRecordService) : ApiControllerBase
{
    /// <summary>Gets every medical record.</summary>
    [HttpGet]
    [ProducesResponseType<IEnumerable<MedicalRecordResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MedicalRecordResponse>>> GetAll(CancellationToken cancellationToken)
        => Ok(await medicalRecordService.GetAllMedicalRecordsAsync(cancellationToken));

    /// <summary>Gets every medical record for a patient.</summary>
    [HttpGet("by-patient/{patientId:int}")]
    [ProducesResponseType<IEnumerable<MedicalRecordResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MedicalRecordResponse>>> GetByPatient(int patientId, CancellationToken cancellationToken)
        => Ok(await medicalRecordService.GetMedicalRecordsByPatientIdAsync(patientId, cancellationToken));

    /// <summary>Gets a single medical record by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType<MedicalRecordResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MedicalRecordResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var record = await medicalRecordService.GetMedicalRecordByIdAsync(id, cancellationToken);
        return record is null ? NotFoundProblem("Medical record", id) : Ok(record);
    }

    /// <summary>Creates a new medical record.</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(MedicalRecordCreateRequest dto, CancellationToken cancellationToken)
    {
        await medicalRecordService.AddMedicalRecordAsync(dto, cancellationToken);
        return StatusCode(StatusCodes.Status201Created);
    }

    /// <summary>Replaces an existing medical record.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, MedicalRecordUpdateRequest dto, CancellationToken cancellationToken)
    {
        if (EnsureMatchingId(id, dto.Id) is { } mismatch)
            return mismatch;

        var response = await medicalRecordService.UpdateMedicalRecordAsync(dto, cancellationToken);
        return response.Success ? NoContent() : ProblemFromServiceResponse(response);
    }

    /// <summary>Updates only the notes of a medical record.</summary>
    [HttpPatch("{id:int}/notes")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateNotes(int id, [FromBody] string notes, CancellationToken cancellationToken)
    {
        await medicalRecordService.UpdateMedicalRecordNotesAsync(id, notes, cancellationToken);
        return NoContent();
    }

    /// <summary>Deletes a medical record.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await medicalRecordService.DeleteMedicalRecordAsync(id, cancellationToken);
        return NoContent();
    }
}
