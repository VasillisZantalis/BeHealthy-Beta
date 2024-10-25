using BeHealthy.Filters;
using BeHealthy.Services.Interfaces;
using BeHealthy.Shared.Models.Dtos.Appointment;
using BeHealthy.Shared.Models.Dtos.Doctor;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BeHealthy.Endpoints.Doctors;

public static class DoctorEndpoints
{
    public static void MapDoctorsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/doctors").RequireAuthorization();

        group.MapGet("", async Task<Results<NotFound, Ok<IEnumerable<DoctorDto>>>>
            ([FromServices] IDoctorService doctorService) =>
        {
            var doctors = await doctorService.GetAllDoctorsAsync();
            return doctors is null
                ? TypedResults.NotFound()
                : TypedResults.Ok(doctors);
        });

        group.MapGet("{id:int}", async Task<Results<NotFound, Ok<DoctorDto>>>
            ([FromServices] IDoctorService doctorService, int id) =>
        {
            var doctors = await doctorService.GetDoctorByIdAsync(id);

            return doctors is null
                ? TypedResults.NotFound()
                : TypedResults.Ok(doctors);
        }).WithName("GetDoctorById");

        group.MapGet("{userId}/appointments", async Task<Results<NotFound, Ok<IEnumerable<AppointmentDto>>>>
            ([FromServices] IDoctorService doctorService, string userId) =>
        {
            var doctorAppointments = await doctorService.GetDoctorAppointmentsByUserIdAsync(userId);

            return doctorAppointments is null
                ? TypedResults.NotFound()
                : TypedResults.Ok(doctorAppointments);
        }).WithName("GetDoctorAppointmentsByUserId");

        group.MapPost("", async Task<Results<Created, BadRequest, UnprocessableEntity>>
           ([FromServices] IDoctorService doctorService,
            DoctorForCreationDto doctorDto) =>
        {
            if (doctorDto is null)
                return TypedResults.BadRequest();

            await doctorService.AddDoctorAsync(doctorDto);

            return TypedResults.Created();
        }).AddEndpointFilter<ValidationFilter<DoctorForCreationDto>>();

        group.MapPut("{id:int}", async Task<Results<NoContent, BadRequest, UnprocessableEntity>>
            ([FromServices] IDoctorService doctorService,
            int id,
            DoctorForUpdateDto doctorDto) =>
        {
            if (doctorDto is null)
                return TypedResults.BadRequest();

            await doctorService.UpdateDoctorAsync(id, doctorDto);

            return TypedResults.NoContent();
        });

        group.MapDelete("{id:int}", async Task<Results<NoContent, BadRequest>>
            ([FromServices] IDoctorService doctorService, int id) =>
        {
            var doctor = await doctorService.GetDoctorByIdAsync(id);

            if (doctor is null) return TypedResults.BadRequest();

            await doctorService.DeleteDoctorAsync(id);

            return TypedResults.NoContent();
        });

    }
}
