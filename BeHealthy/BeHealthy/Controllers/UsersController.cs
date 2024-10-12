using BeHealthy.Shared.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BeHealthy.Controllers;

[Route("api/users")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UsersController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet("doctors")]
    public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetDoctors()
    {
        var doctors = await _userManager.GetUsersInRoleAsync("Doctor");
        return Ok(doctors);
    }

    [HttpGet("patients")]
    public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetPatients()
    {
        var patients = await _userManager.GetUsersInRoleAsync("Patient");
        return Ok(patients);
    }

    [HttpGet("nurses")]
    public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetNurses()
    {
        var nurses = await _userManager.GetUsersInRoleAsync("Nurse");
        return Ok(nurses);
    }

    [HttpGet("staff")]
    public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetStaff()
    {
        var staff = await _userManager.GetUsersInRoleAsync("Staff");
        return Ok(staff);
    }

    [HttpDelete("{id}", Name = nameof(DeleteUser))]
    public async Task<ActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null) return NotFound();
        await _userManager.DeleteAsync(user);

        return NoContent();
    }

}
