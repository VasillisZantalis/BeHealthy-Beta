using BeHealthy.API.Mapping;
using BeHealthy.Application.Mappings;
using BeHealthy.Shared.Dtos.Visit;

namespace BeHealthy.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VisitsController(IVisitService visitService) : ApiControllerBase
{
    /// <summary>Gets every visit.</summary>
    [HttpGet]
    [ProducesResponseType<IEnumerable<VisitResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<VisitResponse>>> GetAll(CancellationToken cancellationToken)
        => Ok((await visitService.GetAllVisitsAsync(cancellationToken)).MapToDto());

    /// <summary>Gets every visit for a patient.</summary>
    [HttpGet("by-patient/{patientId:int}")]
    [ProducesResponseType<IEnumerable<VisitResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<VisitResponse>>> GetByPatient(int patientId, CancellationToken cancellationToken)
        => Ok(await visitService.GetVisitsByPatientIdAsync(patientId, cancellationToken));

    /// <summary>Gets a single visit, including its diagnoses, treatments, and lab results.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType<VisitDetailsResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VisitDetailsResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var visit = await visitService.GetVisitWithDetailsAsync(id, cancellationToken);
        return visit is null ? NotFoundProblem("Visit", id) : Ok(visit.MapToDetailsDto());
    }

    /// <summary>Gets the diagnoses recorded during a visit.</summary>
    [HttpGet("{id:int}/diagnoses")]
    [ProducesResponseType<IEnumerable<DiagnosisResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DiagnosisResponse>>> GetDiagnoses(int id, CancellationToken cancellationToken)
        => Ok((await visitService.GetDiagnosesByVisitIdAsync(id, cancellationToken)).Select(d => d.MapToDto()));

    /// <summary>Gets the treatments prescribed during a visit.</summary>
    [HttpGet("{id:int}/treatments")]
    [ProducesResponseType<IEnumerable<TreatmentResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TreatmentResponse>>> GetTreatments(int id, CancellationToken cancellationToken)
        => Ok((await visitService.GetTreatmentsByVisitIdAsync(id, cancellationToken)).Select(t => t.MapToDto()));

    /// <summary>Gets the lab results recorded during a visit.</summary>
    [HttpGet("{id:int}/lab-results")]
    [ProducesResponseType<IEnumerable<LabResultResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<LabResultResponse>>> GetLabResults(int id, CancellationToken cancellationToken)
        => Ok((await visitService.GetLabResultsByVisitIdAsync(id, cancellationToken)).Select(l => l.MapToDto()));

    /// <summary>Creates a new visit.</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(VisitCreateRequest dto, CancellationToken cancellationToken)
    {
        var response = await visitService.AddVisitAsync(dto, cancellationToken);
        return response.Success ? StatusCode(StatusCodes.Status201Created) : ProblemFromServiceResponse(response);
    }

    /// <summary>Updates an existing visit.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, VisitUpdateRequest dto, CancellationToken cancellationToken)
    {
        if (EnsureMatchingId(id, dto.Id) is { } mismatch)
            return mismatch;

        var response = await visitService.UpdateVisitAsync(dto, cancellationToken);
        return response.Success ? NoContent() : ProblemFromServiceResponse(response);
    }

    /// <summary>Deletes a visit.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await visitService.DeleteVisitAsync(id, cancellationToken);
        return response.Success ? NoContent() : ProblemFromServiceResponse(response);
    }
}
