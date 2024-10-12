using BeHealthy.Filters;
using BeHealthy.Services.Interfaces;
using BeHealthy.Shared.Models.Dtos.Nurse;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BeHealthy.Endpoints.Nurse;

public static class NurseEndpoints
{
    public static void MapNursesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/nurses").RequireAuthorization();

        group.MapGet("", async Task<Results<NotFound, Ok<IEnumerable<NurseDto>>>>
            ([FromServices] INurseService nurseService) =>
        {
            var nurses = await nurseService.GetAllNursesAsync();

            return nurses is null
                ? TypedResults.NotFound()
                : TypedResults.Ok(nurses);
        });

        group.MapGet("{id:int}", async Task<Results<NotFound, Ok<NurseDto>>>
            ([FromServices] INurseService nurseService, int id) =>
        {
            var nurses = await nurseService.GetNurseByIdAsync(id);

            return nurses is null
                ? TypedResults.NotFound()
                : TypedResults.Ok(nurses);
        }).WithName("GetNurseById");

        group.MapPost("", async Task<Results<Created, BadRequest, UnprocessableEntity>>
           ([FromServices] INurseService nurseService,
            NurseForCreationDto nurseDto) =>
        {
            if (nurseDto is null)
                return TypedResults.BadRequest();

            await nurseService.AddNurseAsync(nurseDto);

            return TypedResults.Created();
        }).AddEndpointFilter<ValidationFilter<NurseForCreationDto>>();

        group.MapPut("{id:int}", async Task<Results<NoContent, BadRequest, UnprocessableEntity>>
          ([FromServices] INurseService nurseService,
           NurseForUpdateDto nurseDto) =>
        {
            if (nurseDto is null)
                return TypedResults.BadRequest();

            await nurseService.UpdateNurseAsync(nurseDto);

            return TypedResults.NoContent();
        });

        group.MapDelete("{id:int}", async Task<Results<NoContent, BadRequest>>
            ([FromServices] INurseService nurseService, int id) =>
        {
            var nurse = await nurseService.GetNurseByIdAsync(id);

            if (nurse is null) return TypedResults.BadRequest();

            await nurseService.DeleteNurseAsync(id);

            return TypedResults.NoContent();
        });

    }
}
