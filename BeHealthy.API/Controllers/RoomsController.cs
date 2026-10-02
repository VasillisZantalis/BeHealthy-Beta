using BeHealthy.Shared.Dtos.Room;

namespace BeHealthy.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomsController(IRoomService roomService) : ApiControllerBase
{
    /// <summary>Gets every room.</summary>
    [HttpGet]
    [ProducesResponseType<IEnumerable<RoomResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RoomResponse>>> GetAll(CancellationToken cancellationToken)
        => Ok(await roomService.GetAllRoomsAsync(cancellationToken));

    /// <summary>Gets a single room by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType<RoomResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RoomResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var room = await roomService.GetRoomByIdAsync(id, cancellationToken);
        return room is null ? NotFoundProblem("Room", id) : Ok(room);
    }

    /// <summary>Creates a new room.</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(RoomCreateRequest dto, CancellationToken cancellationToken)
    {
        await roomService.AddRoomAsync(dto, cancellationToken);
        return StatusCode(StatusCodes.Status201Created);
    }

    /// <summary>Updates an existing room.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, RoomUpdateRequest dto, CancellationToken cancellationToken)
    {
        if (EnsureMatchingId(id, dto.Id) is { } mismatch)
            return mismatch;

        var response = await roomService.UpdateRoomAsync(dto, cancellationToken);
        return response.Success ? NoContent() : ProblemFromServiceResponse(response);
    }

    /// <summary>Deletes a room.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await roomService.DeleteRoomAsync(id, cancellationToken);
        return NoContent();
    }
}
