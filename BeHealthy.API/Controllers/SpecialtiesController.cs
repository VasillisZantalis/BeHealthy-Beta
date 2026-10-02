using BeHealthy.Shared.Dtos.Specialty;

namespace BeHealthy.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SpecialtiesController(ISpecialtyService specialtyService) : ApiControllerBase
{
    /// <summary>Gets every specialty.</summary>
    [HttpGet]
    [Authorize(Roles = RoleGroups.AllUsers)]
    [ProducesResponseType<IEnumerable<SpecialtyResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SpecialtyResponse>>> GetAll(CancellationToken cancellationToken)
        => Ok(await specialtyService.GetSpecialtiesAsync(cancellationToken));

    /// <summary>Gets a single specialty by id.</summary>
    [HttpGet("{id:int}")]
    [Authorize(Roles = RoleGroups.AllUsers)]
    [ProducesResponseType<SpecialtyResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SpecialtyResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var specialty = await specialtyService.GetSpecialtyByIdAsync(id, cancellationToken);
        return specialty is null ? NotFoundProblem("Specialty", id) : Ok(specialty);
    }

    /// <summary>Creates a new specialty.</summary>
    [HttpPost]
    [Authorize(Roles = RoleGroups.Administration)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(SpecialtyCreateRequest dto, CancellationToken cancellationToken)
    {
        await specialtyService.AddSpecialtyAsync(dto, cancellationToken);
        return StatusCode(StatusCodes.Status201Created);
    }

    /// <summary>Updates an existing specialty.</summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = RoleGroups.Administration)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, SpecialtyUpdateRequest dto, CancellationToken cancellationToken)
    {
        if (EnsureMatchingId(id, dto.Id) is { } mismatch)
            return mismatch;

        var response = await specialtyService.UpdateSpecialtyAsync(dto, cancellationToken);
        return response.Success ? NoContent() : ProblemFromServiceResponse(response);
    }

    /// <summary>Deletes a specialty.</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = RoleGroups.Administration)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await specialtyService.DeleteSpecialtyAsync(id, cancellationToken);
        return NoContent();
    }
}
