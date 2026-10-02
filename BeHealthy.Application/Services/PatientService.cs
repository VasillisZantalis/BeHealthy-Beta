using BeHealthy.Shared.Locales;
using BeHealthy.Shared.Parameters;

namespace BeHealthy.Application.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IUserService _userService;
    private readonly ITransactionManager _transactionManager;

    public PatientService(
        IPatientRepository patientRepository,
        IAppointmentRepository appointmentRepository,
        IDoctorRepository doctorRepository,
        IUserService userService,
        ITransactionManager transactionManager)
    {
        _patientRepository = patientRepository;
        _appointmentRepository = appointmentRepository;
        _doctorRepository = doctorRepository;
        _userService = userService;
        _transactionManager = transactionManager;
    }

    public async Task<IEnumerable<PatientResponse>> GetAllPatientsAsync(PatientQueryParameters? parameters = null, CancellationToken cancellationToken = default)
    {
        parameters ??= new PatientQueryParameters();
        var queryOptions = new QueryOptions<Patient>
        {
            Predicate = p => (string.IsNullOrEmpty(parameters.SearchTerm) ||
                             p.FirstName.Contains(parameters.SearchTerm) ||
                             p.LastName.Contains(parameters.SearchTerm)),

            Includes = { d => d.User! }
        };

        var patients = await _patientRepository.QueryAsync(queryOptions, cancellationToken);
        return patients.MapToDto();
    }

    public async Task<IEnumerable<PatientSimpleResponse>> GetAllPatientsSimpleAsync(CancellationToken cancellationToken = default)
    {
        var patients = await _patientRepository.GetAllPatientsSimpleAsync(cancellationToken);
        return patients.MapToSimpleDto();
    }

    public async Task<PatientResponse?> GetPatientByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var patient = await _patientRepository.GetByIdWithIncludes(id, cancellationToken, w => w.User!);
        return patient?.MapToDto();
    }

    public async Task<ServiceResponse> AddPatientAsync(PatientCreateRequest patientDto, CancellationToken cancellationToken = default)
    {
        var user = new ApplicationUser
        {
            FirstName = patientDto.FirstName,
            LastName = patientDto.LastName,
            PhoneNumber = patientDto.PhoneNumber,
            Email = patientDto.Email
        };

        // UserManager saves on its own, so the user, its role and the patient need one explicit transaction.
        return await _transactionManager.ExecuteInTransactionAsync(async () =>
        {
            var userCreationResult = await _userService.CreateApplicationUser(user, patientDto.Password, cancellationToken);
            if (!userCreationResult.Success)
                return ServiceResponse.Failed(userCreationResult.ErrorMessage!);

            var addToRoleResult = await _userService.AddUserToRoleAsync(user, UserRole.Patient, cancellationToken);
            if (!addToRoleResult.Success)
                return ServiceResponse.Failed(addToRoleResult.ErrorMessage!);

            patientDto.UserId = user.Id;

            var patient = patientDto.MapToDomain();
            await _patientRepository.AddAsync(patient, cancellationToken);
            await _patientRepository.SaveChangesAsync(cancellationToken);

            return ServiceResponse.Successful();
        }, cancellationToken);
    }

    public async Task<ServiceResponse> UpdatePatientAsync(PatientUpdateRequest patientDto, CancellationToken cancellationToken = default)
    {
        var existingUser = await _userService.GetUserByIdAsync(patientDto.UserId, cancellationToken);
        if (existingUser == null)
            return ServiceResponse.Failed(Resource.NotFound);

        var patient = await _patientRepository.GetByIdAsync(patientDto.Id, cancellationToken);
        if (patient is null)
            return ServiceResponse.Failed(Resource.NotFound);

        existingUser.FirstName = patientDto.FirstName;
        existingUser.LastName = patientDto.LastName;
        existingUser.PhoneNumber = patientDto.PhoneNumber;

        patient.FirstName = patientDto.FirstName;
        patient.LastName = patientDto.LastName;
        patient.Image = patientDto.Image;
        patient.DepartmentId = patientDto.DepartmentId;

        return await _transactionManager.ExecuteInTransactionAsync(async () =>
        {
            var updateUserResult = await _userService.UpdateUserAsync(existingUser, cancellationToken);
            if (!updateUserResult.Success)
                return ServiceResponse.Failed(updateUserResult.ErrorMessage!);

            await _patientRepository.UpdateAsync(patient);
            await _patientRepository.SaveChangesAsync(cancellationToken);

            return ServiceResponse.Successful();
        }, cancellationToken);
    }

    public async Task DeletePatientAsync(int id, CancellationToken cancellationToken = default)
    {
        await _patientRepository.DeletePatientAsync(id, cancellationToken);
        await _patientRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<AppointmentResponse>> GetPatientAppointmentsByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var patientAppointments = await _patientRepository.GetPatientAppointmentsByUserIdAsync(userId, cancellationToken);
        return patientAppointments.MapToDto();
    }

    public async Task<IEnumerable<DoctorResponse>> GetMyDoctorsAsync(string userId, CancellationToken cancellationToken = default)
    {
        var doctors = new List<Doctor>();

        var patient = await _patientRepository.GetByUserIdAsync(userId, cancellationToken);

        if (patient is null)
            return Enumerable.Empty<DoctorResponse>();

        var patientAppointments = await _appointmentRepository.GetAllAppointmentsByPatientIdAsync(patient.Id, cancellationToken);

        var doctorIds = patientAppointments
            .Select(s => s.DoctorId)
            .Distinct()
            .ToList();

        if (doctorIds.Any())
        {
            var queryOptions = new QueryOptions<Doctor>
            {
                Predicate = w => doctorIds.Contains(w.Id),
                Includes = { w => w.User! }
            };

            var treatingDoctors = await _doctorRepository.QueryAsync(queryOptions, cancellationToken);

            doctors.AddRange(treatingDoctors);
        }

        return doctors.MapToDto();
    }

    public async Task<int> GetPatientCountAsync(CancellationToken cancellationToken = default)
    {
        return await _patientRepository.GetCountAsync(cancellationToken);
    }

    public async Task<ProfileResponse?> GetPatientProfileByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var patient = await _patientRepository.GetPatientByUserIdAsync(userId, cancellationToken);

        if (patient is null) return null;

        var profile = new ProfileResponse
        {
            Id = patient.Id,
            UserId = patient.UserId,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Image = patient.Image,
            Email = patient.User?.Email,
            PhoneNumber = patient.User?.PhoneNumber,
        };

        return profile;
    }
}
