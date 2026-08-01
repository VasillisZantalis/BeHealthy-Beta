using BeHealthy.Shared.Dtos.Appointment;
using BeHealthy.Shared.Dtos.Doctor;
using BeHealthy.Shared.Dtos.Patient;
using BeHealthy.Shared.Dtos.User;

namespace BeHealthy.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DoctorsController(IDoctorService doctorService) : ApiControllerBase
{
    /// <summary>Gets a paginated, filterable list of doctors.</summary>
    [HttpGet]
    [ProducesResponseType<PaginatedResult<DoctorDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedResult<DoctorDto>>> GetAll([FromQuery] DoctorQueryParameters parameters)
        => Ok(await doctorService.GetAllDoctorsAsync(parameters));

    /// <summary>Gets a lightweight list of doctors for dropdowns.</summary>
    [HttpGet("simple")]
    [ProducesResponseType<IEnumerable<DoctorSimpleDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DoctorSimpleDto>>> GetAllSimple()
        => Ok(await doctorService.GetAllDoctorsSimpleAsync());

    /// <summary>Gets the total number of doctors.</summary>
    [HttpGet("count")]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> GetCount()
        => Ok(await doctorService.GetDoctorCountAsync());

    /// <summary>Gets the doctor profile for the given user.</summary>
    [HttpGet("profile/{userId}")]
    [ProducesResponseType<ProfileDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProfileDto>> GetProfile(string userId)
    {
        var profile = await doctorService.GetDoctorProfileByUserIdAsync(userId);
        return profile is null ? NotFoundProblem("Doctor profile", userId) : Ok(profile);
    }

    /// <summary>Gets the appointments booked with the doctor for the given user.</summary>
    [HttpGet("{userId}/appointments")]
    [ProducesResponseType<IEnumerable<AppointmentDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointments(string userId)
        => Ok(await doctorService.GetDoctorAppointmentsByUserIdAsync(userId));

    /// <summary>Gets the patients assigned to the doctor for the given user.</summary>
    [HttpGet("{userId}/patients")]
    [ProducesResponseType<IEnumerable<PatientDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PatientDto>>> GetPatients(string userId)
        => Ok(await doctorService.GetMyPatientsAsync(userId));

    /// <summary>Gets a single doctor by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType<DoctorDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DoctorDto>> GetById(int id)
    {
        var doctor = await doctorService.GetDoctorByIdAsync(id);
        return doctor is null ? NotFoundProblem("Doctor", id) : Ok(doctor);
    }

    /// <summary>Creates a new doctor and their user account.</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(DoctorCreateDto dto)
    {
        var response = await doctorService.AddDoctorAsync(dto);
        return response.Success ? StatusCode(StatusCodes.Status201Created) : ProblemFromServiceResponse(response);
    }

    /// <summary>Updates an existing doctor.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, DoctorUpdateDto dto)
    {
        if (EnsureMatchingId(id, dto.Id) is { } mismatch)
            return mismatch;

        var response = await doctorService.UpdateDoctorAsync(dto);
        return response.Success ? NoContent() : ProblemFromServiceResponse(response);
    }

    /// <summary>Deletes a doctor.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id)
    {
        await doctorService.DeleteDoctorAsync(id);
        return NoContent();
    }
}
