using BeHealthy.Shared.Dtos.Prescription;

namespace BeHealthy.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PrescriptionsController(IPrescriptionService prescriptionService) : ApiControllerBase
{
    /// <summary>Gets every prescription.</summary>
    [HttpGet]
    [Authorize(Roles = RoleGroups.AllUsers)]
    [ProducesResponseType<IEnumerable<PrescriptionResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PrescriptionResponse>>> GetAll(CancellationToken cancellationToken)
        => Ok(await prescriptionService.GetAllPrescriptionsAsync(cancellationToken));

    /// <summary>Gets every prescription for a patient.</summary>
    [HttpGet("by-patient/{patientId:int}")]
    [Authorize(Roles = RoleGroups.AllUsers)]
    [ProducesResponseType<IEnumerable<PrescriptionResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PrescriptionResponse>>> GetByPatient(int patientId, CancellationToken cancellationToken)
        => Ok(await prescriptionService.GetPrescriptionsByPatientIdAsync(patientId, cancellationToken));

    /// <summary>Gets a single prescription by id.</summary>
    [HttpGet("{id:int}")]
    [Authorize(Roles = RoleGroups.AllUsers)]
    [ProducesResponseType<PrescriptionResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PrescriptionResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var prescription = await prescriptionService.GetPrescriptionByIdAsync(id, cancellationToken);
        return prescription is null ? NotFoundProblem("Prescription", id) : Ok(prescription);
    }

    /// <summary>Creates a new prescription.</summary>
    [HttpPost]
    [Authorize(Roles = RoleGroups.Prescribers)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(PrescriptionCreateRequest dto, CancellationToken cancellationToken)
    {
        var response = await prescriptionService.AddPrescriptionAsync(dto, cancellationToken);
        return response.Success ? StatusCode(StatusCodes.Status201Created) : ProblemFromServiceResponse(response);
    }

    /// <summary>Updates an existing prescription.</summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = RoleGroups.Prescribers)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, PrescriptionUpdateRequest dto, CancellationToken cancellationToken)
    {
        if (EnsureMatchingId(id, dto.Id) is { } mismatch)
            return mismatch;

        var response = await prescriptionService.UpdatePrescriptionAsync(dto, cancellationToken);
        return response.Success ? NoContent() : ProblemFromServiceResponse(response);
    }

    /// <summary>Deletes a prescription.</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = RoleGroups.Prescribers)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await prescriptionService.DeletePrescriptionAsync(id, cancellationToken);
        return response.Success ? NoContent() : ProblemFromServiceResponse(response);
    }
}
