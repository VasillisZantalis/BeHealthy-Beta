using BeHealthy.Services.Interfaces;
using BeHealthy.Shared.Models.Dtos.Room;
using Microsoft.AspNetCore.Mvc;

namespace BeHealthy.Controllers;

[Route("api/rooms")]
[ApiController]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomsController(IRoomService roomService)
    {
        _roomService = roomService ?? throw new ArgumentNullException(nameof(roomService));
    }

    [HttpGet(Name = nameof(GetAllRooms))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<RoomDto>>> GetAllRooms()
    {
        var rooms = await _roomService.GetAllRoomsAsync();
        return rooms is null ? NotFound() : Ok(rooms);
    }

    [HttpGet("{id:int}", Name = nameof(GetRoomById))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RoomDto>> GetRoomById(int id)
    {
        if (id < 0)
            return BadRequest();

        var room = await _roomService.GetRoomByIdAsync(id);

        return room is null ? NotFound() : Ok(room);
    }

    [HttpPost(Name = nameof(AddRoom))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> AddRoom([FromBody] RoomForCreationDto roomDto)
    {
        if (roomDto is null)
            return BadRequest();

        if (!ModelState.IsValid)
            return new UnprocessableEntityObjectResult(ModelState);

        await _roomService.AddRoomAsync(roomDto);
        return Created();
    }

    [HttpPut("{id:int}", Name = nameof(UpdateRoom))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> UpdateRoom(int id, [FromBody] RoomForUpdateDto roomDto)
    {
        if (id < 0 || roomDto is null)
            return BadRequest();

        if (!ModelState.IsValid)
            return new UnprocessableEntityObjectResult(ModelState);

        await _roomService.UpdateRoomAsync(roomDto);

        return NoContent();
    }

    [HttpDelete("{id:int}", Name = nameof(DeleteRoom))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteRoom(int id)
    {
        if (id < 0)
            return BadRequest();

        await _roomService.DeleteRoomAsync(id);

        return Ok();
    }
}
