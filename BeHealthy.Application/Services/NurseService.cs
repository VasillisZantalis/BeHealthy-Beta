using BeHealthy.Application.Dtos.Doctor;
using BeHealthy.Shared.Locales;

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

    public async Task<IEnumerable<NurseDto>> GetAllNursesAsync()
    {
        var nurses = await _unitOfWork.NurseRepository.GetAllNursesAsync();
        return nurses.MapToDto();
    }

    public async Task<NurseDto?> GetNurseByIdAsync(int id)
    {
        var nurse = await _unitOfWork.NurseRepository.GetByIdAsync(id);
        return nurse?.MapToDto();
    }

    public async Task<ServiceResponse> AddNurseAsync(NurseForCreationDto nurseDto)
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

    public async Task<ServiceResponse> UpdateNurseAsync(NurseForUpdateDto nurseDto)
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

        var nurse = nurseDto.MapToDomain();
        await _unitOfWork.NurseRepository.UpdateAsync(nurse);

        return ServiceResponse.Successful();
    }

    public async Task DeleteNurseAsync(int id)
    {
        await _unitOfWork.NurseRepository.DeleteNurseAsync(id);
    }

    public async Task<IEnumerable<NurseDto>> GetNursesOfPatientByUserId(string userId)
    {
        List<Nurse> nurses = new();

        var patient = await _unitOfWork.PatientRepository.GetByUserIdAsync(userId);

        if (patient is null)
            return Enumerable.Empty<NurseDto>();

        var patientAppointments = await _unitOfWork.AppointmentRepository.GetAllAppointmentsByPatientIdAsync(patient.Id);

        List<int?> nurseIds = patientAppointments
            .Select(s => s.NurseId)
            .Distinct()
            .ToList();

        if (nurseIds.Any())
        {
            var nursesThatTreatPatient = await _unitOfWork.NurseRepository.FindAsync(w => nurseIds.Contains(w.Id));
            nurses.AddRange(nursesThatTreatPatient);
        }

        var distinctNurses = nurses
            .GroupBy(g => g.Id)
            .Select(s => s.First())
            .ToList();

        return distinctNurses.MapToDto();
    }

    public Task<int> GetNurseCountAsync()
    {
        return _unitOfWork.NurseRepository.GetCountAsync();
    }
}


