using BeHealthy.Shared.Dtos.Appointment;
using BeHealthy.Shared.Dtos.Doctor;
using BeHealthy.Shared.Dtos.Nurse;
using BeHealthy.Shared.Dtos.Patient;
using BeHealthy.Shared.Dtos.User;
using BeHealthy.Shared.Extensions;

namespace BeHealthy.API.Controllers;

/// <summary>
/// Endpoints scoped to the signed-in user. The user is taken from the JWT, never from the URL,
/// so callers can only ever read their own data.
/// </summary>
[Route("api/me")]
[ApiController]
public class MeController(
    IAppointmentService appointmentService,
    IDoctorService doctorService,
    IPatientService patientService,
    INurseService nurseService) : ApiControllerBase
{
    /// <summary>Gets the profile of the signed-in doctor, nurse, or patient.</summary>
    [HttpGet("profile")]
    [Authorize(Roles = RoleGroups.AppointmentParticipants)]
    [ProducesResponseType<ProfileResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProfileResponse>> GetProfile(CancellationToken cancellationToken)
    {
        var userId = CurrentUserId;

        var profile = User.GetUserRoleEnum() switch
        {
            UserRole.Doctor => await doctorService.GetDoctorProfileByUserIdAsync(userId, cancellationToken),
            UserRole.Nurse => await nurseService.GetNurseProfileByUserIdAsync(userId, cancellationToken),
            UserRole.Patient => await patientService.GetPatientProfileByUserIdAsync(userId, cancellationToken),
            _ => null
        };

        return profile is null ? NotFoundProblem("Profile", userId) : Ok(profile);
    }

    /// <summary>Gets every appointment the signed-in user takes part in.</summary>
    [HttpGet("appointments")]
    [Authorize(Roles = RoleGroups.AppointmentParticipants)]
    [ProducesResponseType<IEnumerable<AppointmentResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AppointmentResponse>>> GetAppointments(CancellationToken cancellationToken)
        => Ok(await appointmentService.GetAllAppointmentsByUserIdAsync(CurrentUserId, cancellationToken));

    /// <summary>Gets the patients the signed-in doctor has appointments with.</summary>
    [HttpGet("patients")]
    [Authorize(Roles = RoleGroups.Doctor)]
    [ProducesResponseType<IEnumerable<PatientResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PatientResponse>>> GetPatients(CancellationToken cancellationToken)
        => Ok(await doctorService.GetMyPatientsAsync(CurrentUserId, cancellationToken));

    /// <summary>Gets the doctors the signed-in patient has appointments with.</summary>
    [HttpGet("doctors")]
    [Authorize(Roles = RoleGroups.Patient)]
    [ProducesResponseType<IEnumerable<DoctorResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DoctorResponse>>> GetDoctors(CancellationToken cancellationToken)
        => Ok(await patientService.GetMyDoctorsAsync(CurrentUserId, cancellationToken));

    /// <summary>Gets the nurses the signed-in patient has appointments with.</summary>
    [HttpGet("nurses")]
    [Authorize(Roles = RoleGroups.Patient)]
    [ProducesResponseType<IEnumerable<NurseResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<NurseResponse>>> GetNurses(CancellationToken cancellationToken)
        => Ok(await nurseService.GetNursesOfPatientByUserId(CurrentUserId, cancellationToken));

    // [Authorize] guarantees an authenticated principal, and every token we issue carries the user id.
    private string CurrentUserId => User.GetUserId()
        ?? throw new InvalidOperationException("The authenticated user has no user id claim.");
}
