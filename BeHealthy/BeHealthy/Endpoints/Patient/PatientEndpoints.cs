using BeHealthy.Application.Dtos.Appointment;
using BeHealthy.Application.Dtos.Patient;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Filters;
using BeHealthy.Shared.Parameters;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BeHealthy.Endpoints.Patient;

public static class PatientEndpoints
{
    public static void MapPatientEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/patients").RequireAuthorization();

        group.MapGet("", async Task<Results<NotFound, Ok<IEnumerable<PatientDto>>>>
            ([FromServices] IPatientService patientService) =>
        {
            var patients = await patientService.GetAllPatientsAsync(new PatientSearchingParameters());

            return patients is null
                ? TypedResults.NotFound()
                : TypedResults.Ok(patients);
        });

        group.MapGet("{id:int}", async Task<Results<NotFound, Ok<PatientDto>>>
            ([FromServices] IPatientService patientService, int id) =>
        {
            var patients = await patientService.GetPatientByIdAsync(id);

            return patients is null
                ? TypedResults.NotFound()
                : TypedResults.Ok(patients);
        }).WithName("GetPatientById");

        group.MapGet("{userId}/appointments", async Task<Results<NotFound, Ok<IEnumerable<AppointmentDto>>>>
            ([FromServices] IPatientService patientService, string userId) =>
        {
            var patientAppointments = await patientService.GetPatientAppointmentsByUserIdAsync(userId);

            return patientAppointments is null
                ? TypedResults.NotFound()
                : TypedResults.Ok(patientAppointments);
        }).WithName("GetPatientAppointmentsByUserId");

        group.MapPost("", async Task<Results<Created, BadRequest, UnprocessableEntity>>
           ([FromServices] IPatientService patientService,
            PatientForCreationDto patientDto) =>
        {
            if (patientDto is null)
                return TypedResults.BadRequest();

            await patientService.AddPatientAsync(patientDto);

            return TypedResults.Created();
        }).AddEndpointFilter<ValidationFilter<PatientForCreationDto>>();

        group.MapPut("{id:int}", async Task<Results<NoContent, BadRequest, UnprocessableEntity>>
            ([FromServices] IPatientService patientService,
            int id,
            PatientForUpdateDto patientDto) =>
        {
            if (patientDto is null)
                return TypedResults.BadRequest();

            await patientService.UpdatePatientAsync(patientDto);

            return TypedResults.NoContent();
        });

        group.MapDelete("{id:int}", async Task<Results<NoContent, BadRequest>>
            ([FromServices] IPatientService patientService, int id) =>
        {
            var patient = await patientService.GetPatientByIdAsync(id);

            if (patient is null) return TypedResults.BadRequest();

            await patientService.DeletePatientAsync(id);

            return TypedResults.NoContent();
        });

    }
}
