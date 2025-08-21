using BeHealthy.Filters;
using BeHealthy.Application.Dtos.Appointment;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using BeHealthy.Application.Services.Interfaces;

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
            AppointmentCreateDto appointmentDto) =>
        {
            if (appointmentDto is null)
                return TypedResults.BadRequest();

            await appointmentService.AddAppointmentAsync(appointmentDto);

            return TypedResults.Created();
        }).AddEndpointFilter<ValidationFilter<AppointmentCreateDto>>();

        group.MapPut("{id:int}", async Task<Results<NoContent, BadRequest, UnprocessableEntity>>
            ([FromServices] IAppointmentService appointmentService,
            int id,
            AppointmentUpdateDto appointmentDto) =>
        {
            if (appointmentDto is null)
                return TypedResults.BadRequest();

            await appointmentService.UpdateAppointmentAsync(appointmentDto);

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
