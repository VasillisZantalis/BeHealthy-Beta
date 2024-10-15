using BeHealthy.Filters;
using BeHealthy.Shared.Interfaces;
using BeHealthy.Shared.Models.Dtos.Prescription;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BeHealthy.Endpoints.Prescription;

public static class PrescriptionEndpoints
{
    public static void MapPrescriptionsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/prescriptions").RequireAuthorization();

        group.MapGet("", async Task<Results<NotFound, Ok<IEnumerable<PrescriptionDto>>>>
            ([FromServices] IPrescriptionService prescriptionService) =>
        {
            var prescriptions = await prescriptionService.GetAllPrescriptionsAsync();

            return prescriptions is null
                ? TypedResults.NotFound()
                : TypedResults.Ok(prescriptions);
        });

        group.MapGet("{id:int}", async Task<Results<NotFound, Ok<PrescriptionDto>>>
            ([FromServices] IPrescriptionService prescriptionService, int id) =>
        {
            var prescriptions = await prescriptionService.GetPrescriptionByIdAsync(id);

            return prescriptions is null
                ? TypedResults.NotFound()
                : TypedResults.Ok(prescriptions);
        }).WithName("GetPrescriptionById");

        group.MapPost("", async Task<Results<Created, BadRequest, UnprocessableEntity>>
           ([FromServices] IPrescriptionService prescriptionService,
            PrescriptionForCreationDto prescriptionDto) =>
        {
            if (prescriptionDto is null)
                return TypedResults.BadRequest();

            await prescriptionService.AddPrescriptionAsync(prescriptionDto);

            return TypedResults.Created();
        }).AddEndpointFilter<ValidationFilter<PrescriptionForCreationDto>>();

        group.MapPut("{id:int}", async Task<Results<NoContent, BadRequest, UnprocessableEntity>>
          ([FromServices] IPrescriptionService prescriptionService,
           PrescriptionForUpdateDto prescriptionDto) =>
        {
            if (prescriptionDto is null)
                return TypedResults.BadRequest();

            await prescriptionService.UpdatePrescriptionAsync(prescriptionDto);

            return TypedResults.NoContent();
        });

        group.MapDelete("{id:int}", async Task<Results<NoContent, BadRequest>>
            ([FromServices] IPrescriptionService prescriptionService, int id) =>
        {
            var prescription = await prescriptionService.GetPrescriptionByIdAsync(id);

            if (prescription is null) return TypedResults.BadRequest();

            await prescriptionService.DeletePrescriptionAsync(id);

            return TypedResults.NoContent();
        });

    }
}
