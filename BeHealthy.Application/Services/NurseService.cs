using BeHealthy.Application.Common.Helpers;
using BeHealthy.Shared.Locales;
using BeHealthy.Shared.Parameters;

namespace BeHealthy.Application.Services;

public class NurseService : INurseService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;

    public NurseService(IUnitOfWork unitOfWork, IUserService userService)
    {
        _unitOfWork = unitOfWork;
        _userService = userService;
    }

    public async Task<PaginatedResult<NurseResponse>> GetAllNursesAsync(QueryParameters? parameters = null)
    {
        parameters ??= new QueryParameters();
        var queryOptions = new QueryOptions<Nurse>
        {
            Predicate = n => string.IsNullOrEmpty(parameters.SearchTerm) ||
                             n.FirstName.Contains(parameters.SearchTerm) ||
                             n.LastName.Contains(parameters.SearchTerm),

            Includes = [n => n.User!],
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize
        };

        if (!string.IsNullOrWhiteSpace(parameters.OrderBy))
        {
            queryOptions.OrderBy = OrderByHelper.GetOrderByExpression<Nurse>(parameters.OrderBy);
            queryOptions.OrderDescending = parameters.OrderDescending;
        }

        var nurses = await _unitOfWork.NurseRepository.QueryAsync(queryOptions);
        var totalCount = await _unitOfWork.NurseRepository.GetCountAsync();

        return new PaginatedResult<NurseResponse>
        {
            Items = nurses.MapToDto(),
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize,
            TotalCount = totalCount
        };

    }

    public async Task<NurseResponse?> GetNurseByIdAsync(int id)
    {
        var nurse = await _unitOfWork.NurseRepository.GetByIdWithIncludes(id, d => d.User!);
        return nurse?.MapToDto();
    }

    public async Task<ServiceResponse> AddNurseAsync(NurseCreateRequest nurseDto)
    {
        var user = new ApplicationUser
        {
            FirstName = nurseDto.FirstName,
            LastName = nurseDto.LastName,
            PhoneNumber = nurseDto.PhoneNumber,
            Email = nurseDto.Email
        };

        try
        {
            var userCreationResult = await _userService.CreateApplicationUser(user, nurseDto.Password);
            if (!userCreationResult.Success)
                return ServiceResponse.Failed(userCreationResult.ErrorMessage!);

            var addToRoleResult = await _userService.AddUserToRoleAsync(user, UserRole.Nurse);
            if (!addToRoleResult.Success)
                return ServiceResponse.Failed(addToRoleResult.ErrorMessage!);

            nurseDto.UserId = user.Id;
            var nurse = nurseDto.MapToDomain();

            await _unitOfWork.NurseRepository.AddAsync(nurse);

            return ServiceResponse.Successful();
        }
        catch (Exception)
        {
            await _userService.DeleteUserAsync(user);
            return ServiceResponse.Failed();
        }
    }

    public async Task<ServiceResponse> UpdateNurseAsync(NurseUpdateRequest nurseDto)
    {
        var existingUser = await _userService.GetUserByIdAsync(nurseDto.UserId);
        if (existingUser == null)
            return ServiceResponse.Failed(Resource.NotFound);

        existingUser.FirstName = nurseDto.FirstName;
        existingUser.LastName = nurseDto.LastName;
        existingUser.PhoneNumber = nurseDto.PhoneNumber;

        var updateUserResult = await _userService.UpdateUserAsync(existingUser);
        if (!updateUserResult.Success)
            return ServiceResponse.Failed(updateUserResult.ErrorMessage!);

        var nurse = await _unitOfWork.NurseRepository.GetByIdAsync(nurseDto.Id);
        if (nurse is null)
            return ServiceResponse.Failed(Resource.NotFound);

        nurse.FirstName = nurseDto.FirstName;
        nurse.LastName = nurseDto.LastName;
        nurse.Image = nurseDto.Image;
        nurse.DepartmentId = nurseDto.DepartmentId;

        await _unitOfWork.NurseRepository.UpdateAsync(nurse);

        return ServiceResponse.Successful();
    }

    public async Task DeleteNurseAsync(int id)
    {
        await _unitOfWork.NurseRepository.DeleteNurseAsync(id);
    }

    public async Task<IEnumerable<NurseResponse>> GetNursesOfPatientByUserId(string userId)
    {
        List<Nurse> nurses = new();

        var patient = await _unitOfWork.PatientRepository.GetByUserIdAsync(userId);

        if (patient is null)
            return Enumerable.Empty<NurseResponse>();

        var patientAppointments = await _unitOfWork.AppointmentRepository.GetAllAppointmentsByPatientIdAsync(patient.Id);

        List<int?> nurseIds = patientAppointments
            .Select(s => s.NurseId)
            .Distinct()
            .ToList();

        if (nurseIds.Any())
        {
            var nursesThatTreatPatient = await _unitOfWork.NurseRepository.QueryAsync(new QueryOptions<Nurse>
            {
                Predicate = w => nurseIds.Contains(w.Id)
            });

            nurses.AddRange(nursesThatTreatPatient);
        }

        var distinctNurses = nurses
            .GroupBy(g => g.Id)
            .Select(s => s.First())
            .ToList();

        return distinctNurses.MapToDto();
    }

    public async Task<int> GetNurseCountAsync()
    {
        return await _unitOfWork.NurseRepository.GetCountAsync();
    }

    public async Task<ProfileResponse?> GetNurseProfileByUserIdAsync(string userId)
    {
        var nurse = await _unitOfWork.NurseRepository.GetNurseByUserIdAsync(userId);

        if (nurse is null) return null;

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

    public async Task<IEnumerable<NurseSimpleResponse>> GetAllNursesSimpleAsync()
    {
        var nurses = await _unitOfWork.NurseRepository.GetAllAsync();
        return nurses.Select(x => x.MapToSimpleDto()).ToList();
    }
}


