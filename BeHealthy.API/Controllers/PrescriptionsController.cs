using BeHealthy.Shared.Dtos.Prescription;

namespace BeHealthy.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PrescriptionsController(IPrescriptionService prescriptionService) : ApiControllerBase
{
    /// <summary>Gets every prescription.</summary>
    [HttpGet]
    [ProducesResponseType<IEnumerable<PrescriptionDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PrescriptionDto>>> GetAll()
        => Ok(await prescriptionService.GetAllPrescriptionsAsync());

    /// <summary>Gets every prescription for a patient.</summary>
    [HttpGet("by-patient/{patientId:int}")]
    [ProducesResponseType<IEnumerable<PrescriptionDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PrescriptionDto>>> GetByPatient(int patientId)
        => Ok(await prescriptionService.GetPrescriptionsByPatientIdAsync(patientId));

    /// <summary>Gets a single prescription by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType<PrescriptionDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PrescriptionDto>> GetById(int id)
    {
        var prescription = await prescriptionService.GetPrescriptionByIdAsync(id);
        return prescription is null ? NotFoundProblem("Prescription", id) : Ok(prescription);
    }

    /// <summary>Creates a new prescription.</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(PrescriptionCreateDto dto)
    {
        var response = await prescriptionService.AddPrescriptionAsync(dto);
        return response.Success ? StatusCode(StatusCodes.Status201Created) : ProblemFromServiceResponse(response);
    }

    /// <summary>Updates an existing prescription.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, PrescriptionUpdateDto dto)
    {
        if (EnsureMatchingId(id, dto.Id) is { } mismatch)
            return mismatch;

        var response = await prescriptionService.UpdatePrescriptionAsync(dto);
        return response.Success ? NoContent() : ProblemFromServiceResponse(response);
    }

    /// <summary>Deletes a prescription.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await prescriptionService.DeletePrescriptionAsync(id);
        return response.Success ? NoContent() : ProblemFromServiceResponse(response);
    }
}
