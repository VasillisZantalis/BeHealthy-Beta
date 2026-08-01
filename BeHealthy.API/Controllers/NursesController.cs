using BeHealthy.Shared.Dtos.Nurse;
using BeHealthy.Shared.Dtos.User;

namespace BeHealthy.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NursesController(INurseService nurseService) : ApiControllerBase
{
    /// <summary>Gets a paginated, filterable list of nurses.</summary>
    [HttpGet]
    [ProducesResponseType<PaginatedResult<NurseDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedResult<NurseDto>>> GetAll([FromQuery] QueryParameters parameters)
        => Ok(await nurseService.GetAllNursesAsync(parameters));

    /// <summary>Gets a lightweight list of nurses for dropdowns.</summary>
    [HttpGet("simple")]
    [ProducesResponseType<IEnumerable<NurseSimpleDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<NurseSimpleDto>>> GetAllSimple()
        => Ok(await nurseService.GetAllNursesSimpleAsync());

    /// <summary>Gets the total number of nurses.</summary>
    [HttpGet("count")]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> GetCount()
        => Ok(await nurseService.GetNurseCountAsync());

    /// <summary>Gets the nurse profile for the given user.</summary>
    [HttpGet("profile/{userId}")]
    [ProducesResponseType<ProfileDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProfileDto>> GetProfile(string userId)
    {
        var profile = await nurseService.GetNurseProfileByUserIdAsync(userId);
        return profile is null ? NotFoundProblem("Nurse profile", userId) : Ok(profile);
    }

    /// <summary>Gets the nurses assigned to the given patient's user.</summary>
    [HttpGet("by-patient/{userId}")]
    [ProducesResponseType<IEnumerable<NurseDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<NurseDto>>> GetByPatientUserId(string userId)
        => Ok(await nurseService.GetNursesOfPatientByUserId(userId));

    /// <summary>Gets a single nurse by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType<NurseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<NurseDto>> GetById(int id)
    {
        var nurse = await nurseService.GetNurseByIdAsync(id);
        return nurse is null ? NotFoundProblem("Nurse", id) : Ok(nurse);
    }

    /// <summary>Creates a new nurse and their user account.</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(NurseCreateDto dto)
    {
        var response = await nurseService.AddNurseAsync(dto);
        return response.Success ? StatusCode(StatusCodes.Status201Created) : ProblemFromServiceResponse(response);
    }

    /// <summary>Updates an existing nurse.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, NurseUpdateDto dto)
    {
        if (EnsureMatchingId(id, dto.Id) is { } mismatch)
            return mismatch;

        var response = await nurseService.UpdateNurseAsync(dto);
        return response.Success ? NoContent() : ProblemFromServiceResponse(response);
    }

    /// <summary>Deletes a nurse.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id)
    {
        await nurseService.DeleteNurseAsync(id);
        return NoContent();
    }
}
