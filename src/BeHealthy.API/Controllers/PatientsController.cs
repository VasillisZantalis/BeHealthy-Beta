using BeHealthy.Shared.Dtos.Allergy;
using BeHealthy.Shared.Dtos.Appointment;
using BeHealthy.Shared.Dtos.MedicalRecord;
using BeHealthy.Shared.Dtos.Patient;
using BeHealthy.Shared.Dtos.Prescription;
using BeHealthy.Shared.Dtos.Visit;

namespace BeHealthy.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PatientsController(
    IPatientService patientService,
    IAppointmentService appointmentService,
    IAllergyService allergyService,
    IPrescriptionService prescriptionService,
    IMedicalRecordService medicalRecordService,
    IVisitService visitService) : ApiControllerBase
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

    /// <summary>Gets every appointment for a patient.</summary>
    [HttpGet("{id:int}/appointments")]
    [Authorize(Roles = RoleGroups.AllUsers)]
    [ProducesResponseType<IEnumerable<AppointmentResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AppointmentResponse>>> GetAppointments(int id, CancellationToken cancellationToken)
        => Ok(await appointmentService.GetAllAppointmentsByPatientIdAsync(id, cancellationToken));

    /// <summary>Gets every allergy for a patient.</summary>
    [HttpGet("{id:int}/allergies")]
    [Authorize(Roles = RoleGroups.AllUsers)]
    [ProducesResponseType<IEnumerable<AllergyResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AllergyResponse>>> GetAllergies(int id, CancellationToken cancellationToken)
        => Ok(await allergyService.GetAllergiesByPatientIdAsync(id, cancellationToken));

    /// <summary>Gets every prescription for a patient.</summary>
    [HttpGet("{id:int}/prescriptions")]
    [Authorize(Roles = RoleGroups.AllUsers)]
    [ProducesResponseType<IEnumerable<PrescriptionResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PrescriptionResponse>>> GetPrescriptions(int id, CancellationToken cancellationToken)
        => Ok(await prescriptionService.GetPrescriptionsByPatientIdAsync(id, cancellationToken));

    /// <summary>Gets every medical record for a patient.</summary>
    [HttpGet("{id:int}/medical-records")]
    [Authorize(Roles = RoleGroups.AllUsers)]
    [ProducesResponseType<IEnumerable<MedicalRecordResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MedicalRecordResponse>>> GetMedicalRecords(int id, CancellationToken cancellationToken)
        => Ok(await medicalRecordService.GetMedicalRecordsByPatientIdAsync(id, cancellationToken));

    /// <summary>Gets every visit for a patient.</summary>
    [HttpGet("{id:int}/visits")]
    [Authorize(Roles = RoleGroups.AllUsers)]
    [ProducesResponseType<IEnumerable<VisitResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<VisitResponse>>> GetVisits(int id, CancellationToken cancellationToken)
        => Ok(await visitService.GetVisitsByPatientIdAsync(id, cancellationToken));

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
        {
            return mismatch;
        }

        var response = await patientService.UpdatePatientAsync(dto, cancellationToken);
        return response.Success ? NoContent() : ProblemFromServiceResponse(response);
    }

    /// <summary>Deletes a patient.</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = RoleGroups.Administration)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await patientService.DeletePatientAsync(id, cancellationToken);
        return response.Success ? NoContent() : ProblemFromServiceResponse(response);
    }
}
