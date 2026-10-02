using BeHealthy.Application.Common.Helpers;
using BeHealthy.Shared.Locales;
using BeHealthy.Shared.Parameters;

namespace BeHealthy.Application.Services;

public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly ISpecialtyRepository _specialtyRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IUserService _userService;
    private readonly ITransactionManager _transactionManager;

    public DoctorService(
        IDoctorRepository doctorRepository,
        ISpecialtyRepository specialtyRepository,
        IAppointmentRepository appointmentRepository,
        IPatientRepository patientRepository,
        IUserService userService,
        ITransactionManager transactionManager)
    {
        _doctorRepository = doctorRepository;
        _specialtyRepository = specialtyRepository;
        _appointmentRepository = appointmentRepository;
        _patientRepository = patientRepository;
        _userService = userService;
        _transactionManager = transactionManager;
    }

    public async Task<PaginatedResult<DoctorResponse>> GetAllDoctorsAsync(DoctorQueryParameters? parameters = null, CancellationToken cancellationToken = default)
    {
        parameters ??= new DoctorQueryParameters();
        
        var predicate = (Expression<Func<Doctor, bool>>)(d => 
            (string.IsNullOrEmpty(parameters.SearchTerm) ||
             d.FirstName.Contains(parameters.SearchTerm) ||
             d.LastName.Contains(parameters.SearchTerm)) &&
             (!parameters.SpecialtyId.HasValue || d.SpecialtyId == parameters.SpecialtyId));

        var queryOptions = new QueryOptions<Doctor>
        {
            Predicate = predicate,
            Includes = { d => d.User!, d => d.Specialty! },
            PageSize = parameters.PageSize,
            PageNumber = parameters.PageNumber,
        };

        if (!string.IsNullOrWhiteSpace(parameters.OrderBy))
        {
            queryOptions.OrderBy = OrderByHelper.GetOrderByExpression<Doctor>(parameters.OrderBy);
            queryOptions.OrderDescending = parameters.OrderDescending;
        }

        var doctors = await _doctorRepository.QueryAsync(queryOptions, cancellationToken);
        
        var countOptions = new QueryOptions<Doctor>
        {
            Predicate = predicate
        };
        var allDoctors = await _doctorRepository.QueryAsync(countOptions, cancellationToken);
        var totalCount = await _doctorRepository.GetCountAsync(cancellationToken);

        return new PaginatedResult<DoctorResponse>
        {
            Items = doctors.MapToDto(),
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<DoctorResponse?> GetDoctorByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var doctor = await _doctorRepository.GetByIdWithIncludes(id, cancellationToken, d => d.User!);
        return doctor?.MapToDto();
    }

    public async Task<ServiceResponse> AddDoctorAsync(DoctorCreateRequest doctorDto, CancellationToken cancellationToken = default)
    {
        var user = new ApplicationUser
        {
            FirstName = doctorDto.FirstName,
            LastName = doctorDto.LastName,
            PhoneNumber = doctorDto.PhoneNumber,
            Email = doctorDto.Email
        };

        // UserManager saves on its own, so the user, its role and the doctor need one explicit transaction.
        return await _transactionManager.ExecuteInTransactionAsync(async () =>
        {
            var userCreationResult = await _userService.CreateApplicationUser(user, doctorDto.Password, cancellationToken);
            if (!userCreationResult.Success)
                return ServiceResponse.Failed(userCreationResult.ErrorMessage!);

            var addToRoleResult = await _userService.AddUserToRoleAsync(user, UserRole.Doctor, cancellationToken);
            if (!addToRoleResult.Success)
                return ServiceResponse.Failed(addToRoleResult.ErrorMessage!);

            doctorDto.UserId = user.Id;
            var doctor = doctorDto.MapToDomain();

            await _doctorRepository.AddAsync(doctor, cancellationToken);
            await _doctorRepository.SaveChangesAsync(cancellationToken);

            return ServiceResponse.Successful();
        }, cancellationToken);
    }

    public async Task<ServiceResponse> UpdateDoctorAsync(DoctorUpdateRequest doctorDto, CancellationToken cancellationToken = default)
    {
        var existingUser = await _userService.GetUserByIdAsync(doctorDto.UserId, cancellationToken);
        if (existingUser == null)
            return ServiceResponse.Failed(Resource.NotFound);

        var doctor = await _doctorRepository.GetByIdAsync(doctorDto.Id, cancellationToken);
        if (doctor is null)
            return ServiceResponse.Failed(Resource.NotFound);

        if (doctorDto.SpecialtyId.HasValue && !await _specialtyRepository.ExistsAsync(doctorDto.SpecialtyId.Value, cancellationToken))
            return ServiceResponse.Failed(Resource.NotFound);

        existingUser.FirstName = doctorDto.FirstName;
        existingUser.LastName = doctorDto.LastName;
        existingUser.PhoneNumber = doctorDto.PhoneNumber;

        doctor.FirstName = doctorDto.FirstName;
        doctor.LastName = doctorDto.LastName;
        doctor.Image = doctorDto.Image;
        doctor.SpecialtyId = doctorDto.SpecialtyId;
        doctor.DepartmentId = doctorDto.DepartmentId;

        return await _transactionManager.ExecuteInTransactionAsync(async () =>
        {
            var updateUserResult = await _userService.UpdateUserAsync(existingUser, cancellationToken);
            if (!updateUserResult.Success)
                return ServiceResponse.Failed(updateUserResult.ErrorMessage!);

            await _doctorRepository.UpdateAsync(doctor);
            await _doctorRepository.SaveChangesAsync(cancellationToken);

            return ServiceResponse.Successful();
        }, cancellationToken);
    }

    public async Task DeleteDoctorAsync(int id, CancellationToken cancellationToken = default)
    {
        await _doctorRepository.DeleteDoctorAsync(id, cancellationToken);
        await _doctorRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<AppointmentResponse>> GetDoctorAppointmentsByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var doctorAppointments = await _doctorRepository.GetDoctorAppointmentsByUserIdAsync(userId, cancellationToken);
        return doctorAppointments.MapToDto();
    }

    public async Task<ProfileResponse?> GetDoctorProfileByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var doctor = await _doctorRepository.GetDoctorByUserIdAsync(userId, cancellationToken);

        if (doctor is null) return null;

        var profile = new ProfileResponse
        {
            Id = doctor.Id,
            UserId = doctor.UserId,
            FirstName = doctor.FirstName,
            LastName = doctor.LastName,
            Specialty = doctor.Specialty?.Name,
            Image = doctor.Image,
            Email = doctor.User?.Email,
            PhoneNumber = doctor.User?.PhoneNumber,
        };

        return profile;
    }

    public async Task<IEnumerable<PatientResponse>> GetMyPatientsAsync(string userId, CancellationToken cancellationToken = default)
    {
        var patients = new List<Patient>();

        var doctor = await _doctorRepository.GetDoctorByUserIdAsync(userId, cancellationToken);

        if (doctor is null)
            return Enumerable.Empty<PatientResponse>();

        var doctorAppointments = await _appointmentRepository.GetAllAppointmentsByDoctorIdAsync(doctor.Id, cancellationToken);

        List<int> patientIds = doctorAppointments
            .Select(x => x.PatientId)
            .Distinct()
            .ToList();

        if (patientIds.Any())
        {
            var queryOptions = new QueryOptions<Patient>
            {
                Predicate = w => patientIds.Contains(w.Id),
                Includes = { w => w.User! }
            };

            var treatedPatients = await _patientRepository.QueryAsync(queryOptions, cancellationToken);
            patients.AddRange(treatedPatients);
        }

        var isSupervisorDoctor = await _doctorRepository.IsDoctorHeadOfDepartmentAsync(doctor.Id, cancellationToken);

        if (isSupervisorDoctor)
        {
            var departmentId = doctor.DepartmentId ?? 0;
            var departmentPatients = await _patientRepository.GetPatientsByDepartmentIdAsync(departmentId, cancellationToken);

            patients.AddRange(departmentPatients);
        }

        var distinctPatients = patients
            .GroupBy(x => x.Id)
            .Select(x => x.First())
            .ToList();

        return distinctPatients.MapToDto();
    }

    public Task<int> GetDoctorCountAsync(CancellationToken cancellationToken = default)
    {
        return _doctorRepository.GetCountAsync(cancellationToken);
    }

    public async Task<IEnumerable<DoctorSimpleResponse>> GetAllDoctorsSimpleAsync(CancellationToken cancellationToken = default)
    {
        var doctors = await _doctorRepository.GetAllDoctorsSimpleAsync(cancellationToken);

        return doctors.MapToSimpleDto();
    }
}

