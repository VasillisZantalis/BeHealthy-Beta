using BeHealthy.Shared.Dtos.MedicalRecord;

namespace BeHealthy.API.Controllers;

[Route("api/medical-records")]
[ApiController]
public class MedicalRecordsController(IMedicalRecordService medicalRecordService) : ApiControllerBase
{
    /// <summary>Gets every medical record.</summary>
    [HttpGet]
    [ProducesResponseType<IEnumerable<MedicalRecordResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MedicalRecordResponse>>> GetAll()
        => Ok(await medicalRecordService.GetAllMedicalRecordsAsync());

    /// <summary>Gets every medical record for a patient.</summary>
    [HttpGet("by-patient/{patientId:int}")]
    [ProducesResponseType<IEnumerable<MedicalRecordResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MedicalRecordResponse>>> GetByPatient(int patientId)
        => Ok(await medicalRecordService.GetMedicalRecordsByPatientIdAsync(patientId));

    /// <summary>Gets a single medical record by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType<MedicalRecordResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MedicalRecordResponse>> GetById(int id)
    {
        var record = await medicalRecordService.GetMedicalRecordByIdAsync(id);
        return record is null ? NotFoundProblem("Medical record", id) : Ok(record);
    }

    /// <summary>Creates a new medical record.</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(MedicalRecordCreateRequest dto)
    {
        await medicalRecordService.AddMedicalRecordAsync(dto);
        return StatusCode(StatusCodes.Status201Created);
    }

    /// <summary>Replaces an existing medical record.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, MedicalRecordUpdateRequest dto)
    {
        if (EnsureMatchingId(id, dto.Id) is { } mismatch)
            return mismatch;

        await medicalRecordService.UpdateMedicalRecordAsync(dto);
        return NoContent();
    }

    /// <summary>Updates only the notes of a medical record.</summary>
    [HttpPatch("{id:int}/notes")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateNotes(int id, [FromBody] string notes)
    {
        await medicalRecordService.UpdateMedicalRecordNotesAsync(id, notes);
        return NoContent();
    }

    /// <summary>Deletes a medical record.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id)
    {
        await medicalRecordService.DeleteMedicalRecordAsync(id);
        return NoContent();
    }
}
