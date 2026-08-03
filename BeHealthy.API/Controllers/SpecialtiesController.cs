using BeHealthy.Shared.Dtos.Specialty;

namespace BeHealthy.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SpecialtiesController(ISpecialtyService specialtyService) : ApiControllerBase
{
    /// <summary>Gets every specialty.</summary>
    [HttpGet]
    [ProducesResponseType<IEnumerable<SpecialtyResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SpecialtyResponse>>> GetAll()
        => Ok(await specialtyService.GetSpecialtiesAsync());

    /// <summary>Gets a single specialty by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType<SpecialtyResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SpecialtyResponse>> GetById(int id)
    {
        var specialty = await specialtyService.GetSpecialtyByIdAsync(id);
        return specialty is null ? NotFoundProblem("Specialty", id) : Ok(specialty);
    }

    /// <summary>Creates a new specialty.</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(SpecialtyCreateRequest dto)
    {
        await specialtyService.AddSpecialtyAsync(dto);
        return StatusCode(StatusCodes.Status201Created);
    }

    /// <summary>Updates an existing specialty.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, SpecialtyUpdateRequest dto)
    {
        if (EnsureMatchingId(id, dto.Id) is { } mismatch)
            return mismatch;

        await specialtyService.UpdateSpecialtyAsync(dto);
        return NoContent();
    }

    /// <summary>Deletes a specialty.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id)
    {
        await specialtyService.DeleteSpecialtyAsync(id);
        return NoContent();
    }
}
