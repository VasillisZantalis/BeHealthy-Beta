using BeHealthy.Shared.Dtos.Nurse;
using BeHealthy.Shared.Dtos.User;

namespace BeHealthy.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NursesController(INurseService nurseService) : ApiControllerBase
{
    /// <summary>Gets a paginated, filterable list of nurses.</summary>
    [HttpGet]
    [ProducesResponseType<PaginatedResult<NurseResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedResult<NurseResponse>>> GetAll([FromQuery] QueryParameters parameters, CancellationToken cancellationToken)
        => Ok(await nurseService.GetAllNursesAsync(parameters, cancellationToken));

    /// <summary>Gets a lightweight list of nurses for dropdowns.</summary>
    [HttpGet("simple")]
    [ProducesResponseType<IEnumerable<NurseSimpleResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<NurseSimpleResponse>>> GetAllSimple(CancellationToken cancellationToken)
        => Ok(await nurseService.GetAllNursesSimpleAsync(cancellationToken));

    /// <summary>Gets the total number of nurses.</summary>
    [HttpGet("count")]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> GetCount(CancellationToken cancellationToken)
        => Ok(await nurseService.GetNurseCountAsync(cancellationToken));

    /// <summary>Gets the nurse profile for the given user.</summary>
    [HttpGet("profile/{userId}")]
    [ProducesResponseType<ProfileResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProfileResponse>> GetProfile(string userId, CancellationToken cancellationToken)
    {
        var profile = await nurseService.GetNurseProfileByUserIdAsync(userId, cancellationToken);
        return profile is null ? NotFoundProblem("Nurse profile", userId) : Ok(profile);
    }

    /// <summary>Gets the nurses assigned to the given patient's user.</summary>
    [HttpGet("by-patient/{userId}")]
    [ProducesResponseType<IEnumerable<NurseResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<NurseResponse>>> GetByPatientUserId(string userId, CancellationToken cancellationToken)
        => Ok(await nurseService.GetNursesOfPatientByUserId(userId, cancellationToken));

    /// <summary>Gets a single nurse by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType<NurseResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<NurseResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var nurse = await nurseService.GetNurseByIdAsync(id, cancellationToken);
        return nurse is null ? NotFoundProblem("Nurse", id) : Ok(nurse);
    }

    /// <summary>Creates a new nurse and their user account.</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(NurseCreateRequest dto, CancellationToken cancellationToken)
    {
        var response = await nurseService.AddNurseAsync(dto, cancellationToken);
        return response.Success ? StatusCode(StatusCodes.Status201Created) : ProblemFromServiceResponse(response);
    }

    /// <summary>Updates an existing nurse.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, NurseUpdateRequest dto, CancellationToken cancellationToken)
    {
        if (EnsureMatchingId(id, dto.Id) is { } mismatch)
            return mismatch;

        var response = await nurseService.UpdateNurseAsync(dto, cancellationToken);
        return response.Success ? NoContent() : ProblemFromServiceResponse(response);
    }

    /// <summary>Deletes a nurse.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await nurseService.DeleteNurseAsync(id, cancellationToken);
        return NoContent();
    }
}
