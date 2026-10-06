using BeHealthy.Application.Common.Helpers;
using BeHealthy.Shared.Locales;
using BeHealthy.Shared.Parameters;

namespace BeHealthy.Application.Services;

public class NurseService : INurseService
{
    private readonly INurseRepository _nurseRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IUserService _userService;
    private readonly ITransactionManager _transactionManager;

    public NurseService(
        INurseRepository nurseRepository,
        IPatientRepository patientRepository,
        IAppointmentRepository appointmentRepository,
        IUserService userService,
        ITransactionManager transactionManager)
    {
        _nurseRepository = nurseRepository;
        _patientRepository = patientRepository;
        _appointmentRepository = appointmentRepository;
        _userService = userService;
        _transactionManager = transactionManager;
    }

    public async Task<PaginatedResult<NurseResponse>> GetAllNursesAsync(QueryParameters? parameters = null, CancellationToken cancellationToken = default)
    {
        parameters ??= new QueryParameters();
        Expression<Func<Nurse, bool>> predicate = n =>
            string.IsNullOrEmpty(parameters.SearchTerm) ||
            n.FirstName.Contains(parameters.SearchTerm) ||
            n.LastName.Contains(parameters.SearchTerm);

        var queryOptions = new QueryOptions<Nurse>
        {
            Predicate = predicate,
            Includes = [n => n.User!],
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize
        };

        if (!string.IsNullOrWhiteSpace(parameters.OrderBy))
        {
            queryOptions.OrderBy = OrderByHelper.GetOrderByExpression<Nurse>(parameters.OrderBy);
            queryOptions.OrderDescending = parameters.OrderDescending;
        }

        var nurses = await _nurseRepository.QueryAsync(queryOptions, cancellationToken);
        var totalCount = await _nurseRepository.GetCountAsync(predicate, cancellationToken);

        return new PaginatedResult<NurseResponse>
        {
            Items = nurses.MapToDto(),
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize,
            TotalCount = totalCount
        };

    }

    public async Task<NurseResponse?> GetNurseByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var nurse = await _nurseRepository.GetByIdWithIncludes(id, cancellationToken, d => d.User!);
        return nurse?.MapToDto();
    }

    public async Task<ServiceResponse> AddNurseAsync(NurseCreateRequest nurseDto, CancellationToken cancellationToken = default)
    {
        var user = new ApplicationUser
        {
            FirstName = nurseDto.FirstName,
            LastName = nurseDto.LastName,
            PhoneNumber = nurseDto.PhoneNumber,
            Email = nurseDto.Email
        };

        // UserManager saves on its own, so the user, its role and the nurse need one explicit transaction.
        return await _transactionManager.ExecuteInTransactionAsync(async () =>
        {
            var userCreationResult = await _userService.CreateApplicationUser(user, nurseDto.Password, cancellationToken);
            if (!userCreationResult.Success)
            {
                return ServiceResponse.Failed(userCreationResult.ErrorMessage!);
            }

            var addToRoleResult = await _userService.AddUserToRoleAsync(user, UserRole.Nurse, cancellationToken);
            if (!addToRoleResult.Success)
            {
                return ServiceResponse.Failed(addToRoleResult.ErrorMessage!);
            }

            nurseDto.UserId = user.Id;
            var nurse = nurseDto.MapToDomain();

            await _nurseRepository.AddAsync(nurse, cancellationToken);
            await _nurseRepository.SaveChangesAsync(cancellationToken);

            return ServiceResponse.Successful();
        }, cancellationToken);
    }

    public async Task<ServiceResponse> UpdateNurseAsync(NurseUpdateRequest nurseDto, CancellationToken cancellationToken = default)
    {
        // The account is always the nurse's own; the request can't point at another user.
        var nurse = await _nurseRepository.GetByIdWithIncludes(nurseDto.Id, cancellationToken, n => n.User!);
        if (nurse?.User is null)
        {
            return ServiceResponse.Failed(string.Format(Resource.NotFoundEntity, Resource.Nurse));
        }

        var existingUser = nurse.User;

        existingUser.FirstName = nurseDto.FirstName;
        existingUser.LastName = nurseDto.LastName;
        existingUser.PhoneNumber = nurseDto.PhoneNumber;

        nurse.FirstName = nurseDto.FirstName;
        nurse.LastName = nurseDto.LastName;
        nurse.Image = nurseDto.Image;
        nurse.DepartmentId = nurseDto.DepartmentId;

        return await _transactionManager.ExecuteInTransactionAsync(async () =>
        {
            var updateUserResult = await _userService.UpdateUserAsync(existingUser, cancellationToken);
            if (!updateUserResult.Success)
            {
                return ServiceResponse.Failed(updateUserResult.ErrorMessage!);
            }

            await _nurseRepository.UpdateAsync(nurse);
            await _nurseRepository.SaveChangesAsync(cancellationToken);

            return ServiceResponse.Successful();
        }, cancellationToken);
    }

    public async Task<ServiceResponse> DeleteNurseAsync(int id, CancellationToken cancellationToken = default)
    {
        if (!await _nurseRepository.DeleteNurseAsync(id, cancellationToken))
        {
            return ServiceResponse.Failed(string.Format(Resource.NotFoundEntity, Resource.Nurse));
        }

        await _nurseRepository.SaveChangesAsync(cancellationToken);
        return ServiceResponse.Successful();
    }

    public async Task<IEnumerable<NurseResponse>> GetNursesOfPatientByUserId(string userId, CancellationToken cancellationToken = default)
    {
        List<Nurse> nurses = new();

        var patient = await _patientRepository.GetByUserIdAsync(userId, cancellationToken);

        if (patient is null)
        {
            return Enumerable.Empty<NurseResponse>();
        }

        var patientAppointments = await _appointmentRepository.GetAllAppointmentsByPatientIdAsync(patient.Id, cancellationToken);

        List<int?> nurseIds = patientAppointments
            .Select(s => s.NurseId)
            .Distinct()
            .ToList();

        if (nurseIds.Any())
        {
            var nursesThatTreatPatient = await _nurseRepository.QueryAsync(new QueryOptions<Nurse>
            {
                Predicate = w => nurseIds.Contains(w.Id)
            }, cancellationToken);

            nurses.AddRange(nursesThatTreatPatient);
        }

        var distinctNurses = nurses
            .GroupBy(g => g.Id)
            .Select(s => s.First())
            .ToList();

        return distinctNurses.MapToDto();
    }

    public async Task<int> GetNurseCountAsync(CancellationToken cancellationToken = default)
    {
        return await _nurseRepository.GetCountAsync(cancellationToken);
    }

    public async Task<ProfileResponse?> GetNurseProfileByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var nurse = await _nurseRepository.GetNurseByUserIdAsync(userId, cancellationToken);

        if (nurse is null)
        {
            return null;
        }

        var profile = new ProfileResponse
        {
            Id = nurse.Id,
            UserId = nurse.UserId,
            FirstName = nurse.FirstName,
            LastName = nurse.LastName,
            Image = nurse.Image,
            Email = nurse.User?.Email,
            PhoneNumber = nurse.User?.PhoneNumber,
        };

        return profile;
    }

    public async Task<IEnumerable<NurseSimpleResponse>> GetAllNursesSimpleAsync(CancellationToken cancellationToken = default)
    {
        var nurses = await _nurseRepository.GetAllAsync(cancellationToken);
        return nurses.Select(x => x.MapToSimpleDto()).ToList();
    }
}


