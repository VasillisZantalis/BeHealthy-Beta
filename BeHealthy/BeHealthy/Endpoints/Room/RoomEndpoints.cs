using BeHealthy.Filters;
using BeHealthy.Services.Interfaces;
using BeHealthy.Shared.Models.Dtos.Room;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BeHealthy.Endpoints.Room;

public static class RoomEndpoints
{
    public static void MapRoomsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/rooms").RequireAuthorization();

        group.MapGet("", async Task<Results<NotFound, Ok<IEnumerable<RoomDto>>>>
            ([FromServices] IRoomService roomService) =>
        {
            var rooms = await roomService.GetAllRoomsAsync();

            return rooms is null
                ? TypedResults.NotFound()
                : TypedResults.Ok(rooms);
        });

        group.MapGet("{id:int}", async Task<Results<NotFound, Ok<RoomDto>>>
            ([FromServices] IRoomService roomService, int id) =>
        {
            var rooms = await roomService.GetRoomByIdAsync(id);

            return rooms is null
                ? TypedResults.NotFound()
                : TypedResults.Ok(rooms);
        }).WithName("GetRoomById");

        group.MapPost("", async Task<Results<Created, BadRequest, UnprocessableEntity>>
           ([FromServices] IRoomService roomService,
            RoomForCreationDto roomDto) =>
        {
            if (roomDto is null)
                return TypedResults.BadRequest();

            await roomService.AddRoomAsync(roomDto);

            return TypedResults.Created();
        }).AddEndpointFilter<ValidationFilter<RoomForCreationDto>>();

        group.MapPut("{id:int}", async Task<Results<NoContent, BadRequest, UnprocessableEntity>>
            ([FromServices] IRoomService roomService,
            int id,
            RoomForUpdateDto roomDto) =>
        {
            if (roomDto is null)
                return TypedResults.BadRequest();

            await roomService.UpdateRoomAsync(id, roomDto);

            return TypedResults.NoContent();
        });

        group.MapDelete("{id:int}", async Task<Results<NoContent, BadRequest>>
            ([FromServices] IRoomService roomService, int id) =>
        {
            var room = await roomService.GetRoomByIdAsync(id);

            if (room is null) return TypedResults.BadRequest();

            await roomService.DeleteRoomAsync(id);

            return TypedResults.NoContent();
        });

    }
}
