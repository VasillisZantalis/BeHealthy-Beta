using BeHealthy.Shared.Dtos.Room;

namespace BeHealthy.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomsController(IRoomService roomService) : ApiControllerBase
{
    /// <summary>Gets every room.</summary>
    [HttpGet]
    [ProducesResponseType<IEnumerable<RoomDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RoomDto>>> GetAll()
        => Ok(await roomService.GetAllRoomsAsync());

    /// <summary>Gets a single room by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType<RoomDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RoomDto>> GetById(int id)
    {
        var room = await roomService.GetRoomByIdAsync(id);
        return room is null ? NotFoundProblem("Room", id) : Ok(room);
    }

    /// <summary>Creates a new room.</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(RoomCreateDto dto)
    {
        await roomService.AddRoomAsync(dto);
        return StatusCode(StatusCodes.Status201Created);
    }

    /// <summary>Updates an existing room.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, RoomUpdateDto dto)
    {
        if (EnsureMatchingId(id, dto.Id) is { } mismatch)
            return mismatch;

        await roomService.UpdateRoomAsync(dto);
        return NoContent();
    }

    /// <summary>Deletes a room.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id)
    {
        await roomService.DeleteRoomAsync(id);
        return NoContent();
    }
}
