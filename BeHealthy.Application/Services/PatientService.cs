using BeHealthy.Shared.Locales;
using BeHealthy.Shared.Parameters;

namespace BeHealthy.Application.Services;

public class PatientService : IPatientService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;

    public PatientService(IUnitOfWork unitOfWork, IUserService userService)
    {
        _unitOfWork = unitOfWork;
        _userService = userService;
    }

    public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync(PatientQueryParameters? parameters = null)
    {
        parameters ??= new PatientQueryParameters();
        var queryOptions = new QueryOptions<Patient>
        {
            Predicate = p => (string.IsNullOrEmpty(parameters.SearchTerm) ||
                             p.FirstName.Contains(parameters.SearchTerm) ||
                             p.LastName.Contains(parameters.SearchTerm)),

            Includes = { d => d.User! }
        };

        var patients = await _unitOfWork.PatientRepository.QueryAsync(queryOptions);
        return patients.MapToDto();
    }

    public async Task<IEnumerable<PatientSimpleDto>> GetAllPatientsSimpleAsync()
    {
        var patients = await _unitOfWork.PatientRepository.GetAllPatientsSimpleAsync();
        return patients.MapToSimpleDto();
    }

    public async Task<PatientDto?> GetPatientByIdAsync(int id)
    {
        var patient = await _unitOfWork.PatientRepository.GetByIdWithIncludes(id, w => w.User!);
        return patient?.MapToDto();
    }

    public async Task<ServiceResponse> AddPatientAsync(PatientCreateDto patientDto)
    {
        var user = new ApplicationUser
        {
            FirstName = patientDto.FirstName,
            LastName = patientDto.LastName,
            PhoneNumber = patientDto.PhoneNumber,
            Email = patientDto.Email
        };

        try
        {
            var userCreationResult = await _userService.CreateApplicationUser(user, patientDto.Password);
            if (!userCreationResult.Success)
                return ServiceResponse.Failed(userCreationResult.ErrorMessage!);

            var addToRoleResult = await _userService.AddUserToRoleAsync(user, UserRole.Patient);
            if (!addToRoleResult.Success)
                return ServiceResponse.Failed(addToRoleResult.ErrorMessage!);

            patientDto.UserId = user.Id;

            var patient = patientDto.MapToDomain();
            await _unitOfWork.PatientRepository.AddAsync(patient);

            return ServiceResponse.Successful();
        }
        catch (Exception)
        {
            await _userService.DeleteUserAsync(user);
            return ServiceResponse.Failed();
        }
    }

    public async Task<ServiceResponse> UpdatePatientAsync(PatientUpdateDto patientDto)
    {
        var existingUser = await _userService.GetUserByIdAsync(patientDto.UserId);
        if (existingUser == null)
            return ServiceResponse.Failed(Resource.NotFound);

        existingUser.FirstName = patientDto.FirstName;
        existingUser.LastName = patientDto.LastName;
        existingUser.PhoneNumber = patientDto.PhoneNumber;

        var updateUserResult = await _userService.UpdateUserAsync(existingUser);
        if (!updateUserResult.Success)
            return ServiceResponse.Failed(updateUserResult.ErrorMessage!);

        var patient = await _unitOfWork.PatientRepository.GetByIdAsync(patientDto.Id);
        if (patient is null)
            return ServiceResponse.Failed(Resource.NotFound);

        patient.FirstName = patientDto.FirstName;
        patient.LastName = patientDto.LastName;
        patient.Image = patientDto.Image;
        patient.DepartmentId = patientDto.DepartmentId;

        await _unitOfWork.PatientRepository.UpdateAsync(patient);

        return ServiceResponse.Successful();
    }

    public async Task DeletePatientAsync(int id)
    {
        await _unitOfWork.PatientRepository.DeletePatientAsync(id);
    }

    public async Task<IEnumerable<AppointmentDto>> GetPatientAppointmentsByUserIdAsync(string userId)
    {
        var patientAppointments = await _unitOfWork.PatientRepository.GetPatientAppointmentsByUserIdAsync(userId);
        return patientAppointments.MapToDto();
    }

    public async Task<IEnumerable<DoctorDto>> GetMyDoctorsAsync(string userId)
    {
        var doctors = new List<Doctor>();

        var patient = await _unitOfWork.PatientRepository.GetByUserIdAsync(userId);

        if (patient is null)
            return Enumerable.Empty<DoctorDto>();

        var patientAppointments = await _unitOfWork.AppointmentRepository.GetAllAppointmentsByPatientIdAsync(patient.Id);

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

            var treatingDoctors = await _unitOfWork.DoctorRepository.QueryAsync(queryOptions);

            doctors.AddRange(treatingDoctors);
        }

        return doctors.MapToDto();
    }

    public async Task<int> GetPatientCountAsync()
    {
        return await _unitOfWork.PatientRepository.GetCountAsync();
    }

    public async Task<ProfileDto?> GetPatientProfileByUserIdAsync(string userId)
    {
        var patient = await _unitOfWork.PatientRepository.GetPatientByUserIdAsync(userId);

        if (patient is null) return null;

        var profile = new ProfileDto
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
