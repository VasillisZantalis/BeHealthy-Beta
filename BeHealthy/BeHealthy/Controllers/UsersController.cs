using BeHealthy.Shared.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BeHealthy.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UsersController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    // GET: api/users/doctors
    [HttpGet("doctors")]
    public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetDoctors()
    {
        var doctors = await _userManager.GetUsersInRoleAsync("Doctor");
        return Ok(doctors);
    }

    // GET: api/users/patients
    [HttpGet("patients")]
    public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetPatients()
    {
        var patients = await _userManager.GetUsersInRoleAsync("Patient");
        return Ok(patients);
    }

}
