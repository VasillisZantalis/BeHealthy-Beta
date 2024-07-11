using BeHealthy.Services.Interfaces;
using BeHealthy.Shared.Models.Dtos.Nurse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeHealthy.Controllers;

[Route("api/nurses")]
[ApiController]
[Authorize]
public class NursesController : ControllerBase
{
    private readonly INurseService _nurseService;

    public NursesController(INurseService nurseService)
    {
        _nurseService = nurseService ?? throw new ArgumentNullException(nameof(nurseService));
    }

    [HttpGet(Name = nameof(GetAllNurses))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<NurseDto>>> GetAllNurses()
    {
        var nurses = await _nurseService.GetAllNursesAsync();

        return nurses is null ? NotFound() : Ok(nurses);
    }

    [HttpGet("{id:int}", Name = nameof(GetNurseById))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<NurseDto>> GetNurseById(int id)
    {
        if (id < 0)
            return BadRequest();

        var nurse = await _nurseService.GetNurseByIdAsync(id);

        return nurse is null ? NotFound() : Ok(nurse);
    }

    [HttpPost(Name = nameof(AddNurse))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> AddNurse(NurseForCreationDto nurseDto)
    {
        if (nurseDto is null)
            return BadRequest();

        if (!ModelState.IsValid)
            return new UnprocessableEntityObjectResult(ModelState);

        await _nurseService.AddNurseAsync(nurseDto);
        return Created();
    }

    [HttpPut("{id:int}", Name = nameof(UpdateNurse))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> UpdateNurse(int id, [FromBody] NurseForUpdateDto nurseDto)
    {
        if (id < 0 || nurseDto is null)
            return BadRequest();

        if (!ModelState.IsValid)
            return new UnprocessableEntityObjectResult(ModelState);

        await _nurseService.UpdateNurseAsync(nurseDto);

        return NoContent();
    }

    [HttpDelete("{id:int}", Name = nameof(DeleteNurse))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteNurse(int id)
    {
        if (id < 0)
            return BadRequest();

        await _nurseService.DeleteNurseAsync(id);

        return Ok();
    }
}
