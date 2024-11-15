using BeHealthy.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BeHealthy.Endpoints.User;

public static class UserEndpoints
{
    public static void MapUsersEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/users").RequireAuthorization();

        group.MapGet("doctors", async (UserManager<ApplicationUser> userManager) =>
        {
            var doctors = await userManager.GetUsersInRoleAsync("Doctor");
            return TypedResults.Ok(doctors);
        });

        group.MapGet("patients", async (UserManager<ApplicationUser> userManager) =>
        {
            var patients = await userManager.GetUsersInRoleAsync("Patient");
            return TypedResults.Ok(patients);
        });

        group.MapGet("nurses", async (UserManager<ApplicationUser> userManager) =>
        {
            var nurses = await userManager.GetUsersInRoleAsync("Nurse");
            return TypedResults.Ok(nurses);
        });

        group.MapGet("staff", async (UserManager<ApplicationUser> userManager) =>
        {
            var staff = await userManager.GetUsersInRoleAsync("Staff");
            return TypedResults.Ok(staff);
        });

        group.MapDelete("{id}", async (string id, UserManager<ApplicationUser> userManager) =>
        {
            var user = await userManager.FindByIdAsync(id);
            if (user is null) return Results.NotFound();
            await userManager.DeleteAsync(user);
            return TypedResults.NoContent();
        });
    }
}
