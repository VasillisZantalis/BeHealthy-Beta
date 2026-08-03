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
    [ProducesResponseType<IEnumerable<PatientResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PatientResponse>>> GetAll([FromQuery] PatientQueryParameters parameters)
        => Ok(await patientService.GetAllPatientsAsync(parameters));

    /// <summary>Gets a lightweight list of patients for dropdowns.</summary>
    [HttpGet("simple")]
    [ProducesResponseType<IEnumerable<PatientSimpleResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PatientSimpleResponse>>> GetAllSimple()
        => Ok(await patientService.GetAllPatientsSimpleAsync());

    /// <summary>Gets the total number of patients.</summary>
    [HttpGet("count")]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> GetCount()
        => Ok(await patientService.GetPatientCountAsync());

    /// <summary>Gets the patient profile for the given user.</summary>
    [HttpGet("profile/{userId}")]
    [ProducesResponseType<ProfileResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProfileResponse>> GetProfile(string userId)
    {
        var profile = await patientService.GetPatientProfileByUserIdAsync(userId);
        return profile is null ? NotFoundProblem("Patient profile", userId) : Ok(profile);
    }

    /// <summary>Gets the appointments booked by the given user.</summary>
    [HttpGet("{userId}/appointments")]
    [ProducesResponseType<IEnumerable<AppointmentResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AppointmentResponse>>> GetAppointments(string userId)
        => Ok(await patientService.GetPatientAppointmentsByUserIdAsync(userId));

    /// <summary>Gets the doctors assigned to the given user.</summary>
    [HttpGet("{userId}/doctors")]
    [ProducesResponseType<IEnumerable<DoctorResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DoctorResponse>>> GetDoctors(string userId)
        => Ok(await patientService.GetMyDoctorsAsync(userId));

    /// <summary>Gets a single patient by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType<PatientResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PatientResponse>> GetById(int id)
    {
        var patient = await patientService.GetPatientByIdAsync(id);
        return patient is null ? NotFoundProblem("Patient", id) : Ok(patient);
    }

    /// <summary>Creates a new patient and their user account.</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(PatientCreateRequest dto)
    {
        var response = await patientService.AddPatientAsync(dto);
        return response.Success ? StatusCode(StatusCodes.Status201Created) : ProblemFromServiceResponse(response);
    }

    /// <summary>Updates an existing patient.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, PatientUpdateRequest dto)
    {
        if (EnsureMatchingId(id, dto.Id) is { } mismatch)
            return mismatch;

        var response = await patientService.UpdatePatientAsync(dto);
        return response.Success ? NoContent() : ProblemFromServiceResponse(response);
    }

    /// <summary>Deletes a patient.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id)
    {
        await patientService.DeletePatientAsync(id);
        return NoContent();
    }
}
