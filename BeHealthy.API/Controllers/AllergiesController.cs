using BeHealthy.Shared.Dtos.Allergy;

namespace BeHealthy.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AllergiesController(IAllergyService allergyService) : ApiControllerBase
{
    /// <summary>Gets every allergy for a patient.</summary>
    [HttpGet("by-patient/{patientId:int}")]
    [ProducesResponseType<IEnumerable<AllergyResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AllergyResponse>>> GetByPatient(int patientId, CancellationToken cancellationToken)
        => Ok(await allergyService.GetAllergiesByPatientIdAsync(patientId, cancellationToken));

    /// <summary>Gets a single allergy by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType<AllergyResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AllergyResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var allergy = await allergyService.GetAllergyByIdAsync(id, cancellationToken);
        return allergy is null ? NotFoundProblem("Allergy", id) : Ok(allergy);
    }

    /// <summary>Adds a new allergy.</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(AllergyCreateRequest dto, CancellationToken cancellationToken)
    {
        var response = await allergyService.AddAllergyAsync(dto, cancellationToken);
        return response.Success ? StatusCode(StatusCodes.Status201Created) : ProblemFromServiceResponse(response);
    }

    /// <summary>Updates an existing allergy.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, AllergyUpdateRequest dto, CancellationToken cancellationToken)
    {
        if (EnsureMatchingId(id, dto.Id) is { } mismatch)
            return mismatch;

        var response = await allergyService.UpdateAllergyAsync(dto, cancellationToken);
        return response.Success ? NoContent() : ProblemFromServiceResponse(response);
    }

    /// <summary>Deletes an allergy.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await allergyService.DeleteAllergyAsync(id, cancellationToken);
        return response.Success ? NoContent() : ProblemFromServiceResponse(response);
    }
}
