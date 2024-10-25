using BeHealthy.Filters;
using BeHealthy.Services.Interfaces;
using BeHealthy.Shared.Models.Dtos.Appointment;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BeHealthy.Endpoints.Appointments;

public static class AppointmentsEndpoints
{
    public static void MapAppointmentsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/appointments").RequireAuthorization();

        group.MapGet("", async Task<Results<NotFound, Ok<IEnumerable<AppointmentDto>>>>
            ([FromServices] IAppointmentService appointmentService) =>
        {
            var appointments = await appointmentService.GetAllAppointmentsAsync();

            return appointments is null
                ? TypedResults.NotFound()
                : TypedResults.Ok(appointments);
        });

        group.MapGet("{id:int}", async Task<Results<NotFound, Ok<AppointmentDto>>>
            ([FromServices] IAppointmentService appointmentService, int id) =>
        {
            var appointments = await appointmentService.GetAppointmentByIdAsync(id);

            return appointments is null
                ? TypedResults.NotFound()
                : TypedResults.Ok(appointments);
        }).WithName("GetAppointmentById");

        group.MapPost("", async Task<Results<Created, BadRequest, UnprocessableEntity>>
           ([FromServices] IAppointmentService appointmentService,
            AppointmentForCreationDto appointmentDto) =>
        {
            if (appointmentDto is null)
                return TypedResults.BadRequest();

            await appointmentService.AddAppointmentAsync(appointmentDto);

            return TypedResults.Created();
        }).AddEndpointFilter<ValidationFilter<AppointmentForCreationDto>>();

        group.MapPut("{id:int}", async Task<Results<NoContent, BadRequest, UnprocessableEntity>>
            ([FromServices] IAppointmentService appointmentService,
            int id,
            AppointmentForUpdateDto appointmentDto) =>
        {
            if (appointmentDto is null)
                return TypedResults.BadRequest();

            await appointmentService.UpdateAppointmentAsync(id, appointmentDto);

            return TypedResults.NoContent();
        });

        group.MapDelete("{id:int}", async Task<Results<NoContent, BadRequest>>
            ([FromServices] IAppointmentService appointmentService, int id) =>
        {
            var appointment = await appointmentService.GetAppointmentByIdAsync(id);

            if (appointment is null) return TypedResults.BadRequest();

            await appointmentService.DeleteAppointmentAsync(id);

            return TypedResults.NoContent();
        });

    }
}
