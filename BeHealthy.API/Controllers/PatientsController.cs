using BeHealthy.Shared.Dtos.Appointment;
using BeHealthy.Shared.Dtos.Doctor;
using BeHealthy.Shared.Dtos.Patient;
using BeHealthy.Shared.Dtos.User;

namespace BeHealthy.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PatientsController(IPatientService patientService) : ApiControllerBase
{
    /// <summary>Gets a filterable list of patients.</summary>
    [HttpGet]
    [Authorize(Roles = RoleGroups.AllUsers)]
    [ProducesResponseType<IEnumerable<PatientResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PatientResponse>>> GetAll([FromQuery] PatientQueryParameters parameters, CancellationToken cancellationToken)
        => Ok(await patientService.GetAllPatientsAsync(parameters, cancellationToken));

    /// <summary>Gets a lightweight list of patients for dropdowns.</summary>
    [HttpGet("simple")]
    [Authorize(Roles = RoleGroups.AllUsers)]
    [ProducesResponseType<IEnumerable<PatientSimpleResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PatientSimpleResponse>>> GetAllSimple(CancellationToken cancellationToken)
        => Ok(await patientService.GetAllPatientsSimpleAsync(cancellationToken));

    /// <summary>Gets the total number of patients.</summary>
    [HttpGet("count")]
    [Authorize(Roles = RoleGroups.AllUsers)]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> GetCount(CancellationToken cancellationToken)
        => Ok(await patientService.GetPatientCountAsync(cancellationToken));

    /// <summary>Gets the patient profile for the given user.</summary>
    [HttpGet("profile/{userId}")]
    [Authorize(Roles = RoleGroups.AllUsers)]
    [ProducesResponseType<ProfileResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProfileResponse>> GetProfile(string userId, CancellationToken cancellationToken)
    {
        var profile = await patientService.GetPatientProfileByUserIdAsync(userId, cancellationToken);
        return profile is null ? NotFoundProblem("Patient profile", userId) : Ok(profile);
    }

    /// <summary>Gets the appointments booked by the given user.</summary>
    [HttpGet("{userId}/appointments")]
    [Authorize(Roles = RoleGroups.AllUsers)]
    [ProducesResponseType<IEnumerable<AppointmentResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AppointmentResponse>>> GetAppointments(string userId, CancellationToken cancellationToken)
        => Ok(await patientService.GetPatientAppointmentsByUserIdAsync(userId, cancellationToken));

    /// <summary>Gets the doctors assigned to the given user.</summary>
    [HttpGet("{userId}/doctors")]
    [Authorize(Roles = RoleGroups.AllUsers)]
    [ProducesResponseType<IEnumerable<DoctorResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DoctorResponse>>> GetDoctors(string userId, CancellationToken cancellationToken)
        => Ok(await patientService.GetMyDoctorsAsync(userId, cancellationToken));

    /// <summary>Gets a single patient by id.</summary>
    [HttpGet("{id:int}")]
    [Authorize(Roles = RoleGroups.AllUsers)]
    [ProducesResponseType<PatientResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PatientResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var patient = await patientService.GetPatientByIdAsync(id, cancellationToken);
        return patient is null ? NotFoundProblem("Patient", id) : Ok(patient);
    }

    /// <summary>Creates a new patient and their user account.</summary>
    [HttpPost]
    [Authorize(Roles = RoleGroups.Administration)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(PatientCreateRequest dto, CancellationToken cancellationToken)
    {
        var response = await patientService.AddPatientAsync(dto, cancellationToken);
        return response.Success ? StatusCode(StatusCodes.Status201Created) : ProblemFromServiceResponse(response);
    }

    /// <summary>Updates an existing patient.</summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = RoleGroups.Administration)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, PatientUpdateRequest dto, CancellationToken cancellationToken)
    {
        if (EnsureMatchingId(id, dto.Id) is { } mismatch)
            return mismatch;

        var response = await patientService.UpdatePatientAsync(dto, cancellationToken);
        return response.Success ? NoContent() : ProblemFromServiceResponse(response);
    }

    /// <summary>Deletes a patient.</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = RoleGroups.Administration)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await patientService.DeletePatientAsync(id, cancellationToken);
        return NoContent();
    }
}
