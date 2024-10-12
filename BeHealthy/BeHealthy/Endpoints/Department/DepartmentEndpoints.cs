using BeHealthy.Filters;
using BeHealthy.Services.Interfaces;
using BeHealthy.Shared.Models.Dtos.Department;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BeHealthy.Endpoints.Department;

public static class DepartmentEndpoints
{
    public static void MapDepartmentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/departments").RequireAuthorization();

        group.MapGet("", async Task<Results<NotFound, Ok<IEnumerable<DepartmentDto>>>>
            ([FromServices] IDepartmentService departmentService) =>
        {
            var departments = await departmentService.GetAllDepartmentsAsync();

            return departments is null
                ? TypedResults.NotFound()
                : TypedResults.Ok(departments);
        });

        group.MapGet("{id:int}", async Task<Results<NotFound, Ok<DepartmentDto>>>
            ([FromServices] IDepartmentService departmentService, int id) =>
        {
            var departments = await departmentService.GetDepartmentByIdAsync(id);

            return departments is null
                ? TypedResults.NotFound()
                : TypedResults.Ok(departments);
        }).WithName("GetDepartmentById");

        group.MapPost("", async Task<Results<Created, BadRequest, UnprocessableEntity>>
           ([FromServices] IDepartmentService departmentService,
            DepartmentForCreationDto departmentDto) =>
        {
            if (departmentDto is null)
                return TypedResults.BadRequest();

            await departmentService.AddDepartmentAsync(departmentDto);

            return TypedResults.Created();
        }).AddEndpointFilter<ValidationFilter<DepartmentForCreationDto>>();

        group.MapPut("{id:int}", async Task<Results<NoContent, BadRequest, UnprocessableEntity>>
          ([FromServices] IDepartmentService departmentService,
           DepartmentForUpdateDto departmentDto) =>
        {
            if (departmentDto is null)
                return TypedResults.BadRequest();

            await departmentService.UpdateDepartmentAsync(departmentDto);

            return TypedResults.NoContent();
        });

        group.MapDelete("{id:int}", async Task<Results<NoContent, BadRequest>>
            ([FromServices] IDepartmentService departmentService, int id) =>
        {
            var department = await departmentService.GetDepartmentByIdAsync(id);

            if (department is null) return TypedResults.BadRequest();

            await departmentService.DeleteDepartmentAsync(id);

            return TypedResults.NoContent();
        });

    }
}
