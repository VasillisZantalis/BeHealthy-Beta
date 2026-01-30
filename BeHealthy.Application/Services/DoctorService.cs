using BeHealthy.Shared.Locales;
using BeHealthy.Shared.Parameters;

namespace BeHealthy.Application.Services;

public class DoctorService : IDoctorService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;

    public DoctorService(IUnitOfWork unitOfWork, IUserService userService)
    {
        _unitOfWork = unitOfWork;
        _userService = userService;
    }

    public async Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync(DoctorQueryParameters? parameters = null)
    {
        parameters ??= new DoctorQueryParameters();
        var queryOptions = new QueryOptions<Doctor>
        {
            Predicate = d => (string.IsNullOrEmpty(parameters.SearchTerm) ||
                             d.FirstName.Contains(parameters.SearchTerm) ||
                             d.LastName.Contains(parameters.SearchTerm)) &&
                             (!parameters.SpecialtyId.HasValue || d.SpecialtyId == parameters.SpecialtyId),

            Includes = { d => d.User!, d => d.Specialty! },
            PageSize = parameters.PageSize,
            PageNumber = parameters.PageNumber
        };

        var doctors = await _unitOfWork.DoctorRepository.QueryAsync(queryOptions);
        return doctors.MapToDto();
    }

    public async Task<DoctorDto?> GetDoctorByIdAsync(int id)
    {
        var doctor = await _unitOfWork.DoctorRepository.GetByIdWithIncludes(id, d => d.User!);
        return doctor?.MapToDto();
    }

    public async Task<ServiceResponse> AddDoctorAsync(DoctorCreateDto doctorDto)
    {
        var user = new ApplicationUser
        {
            FirstName = doctorDto.FirstName,
            LastName = doctorDto.LastName,
            PhoneNumber = doctorDto.PhoneNumber,
            Email = doctorDto.Email
        };

        try
        {
            var userCreationResult = await _userService.CreateApplicationUser(user, doctorDto.Password);
            if (!userCreationResult.Success)
                return ServiceResponse.Failed(userCreationResult.ErrorMessage!);

            var addToRoleResult = await _userService.AddUserToRoleAsync(user, UserRole.Doctor);
            if (!addToRoleResult.Success)
                return ServiceResponse.Failed(addToRoleResult.ErrorMessage!);

            doctorDto.UserId = user.Id;
            var doctor = doctorDto.MapToDomain();

            await _unitOfWork.DoctorRepository.AddAsync(doctor);

            return ServiceResponse.Successful();
        }
        catch (Exception)
        {
            await _userService.DeleteUserAsync(user);
            return ServiceResponse.Failed();
        }
    }

    public async Task<ServiceResponse> UpdateDoctorAsync(DoctorUpdateDto doctorDto)
    {
        var existingUser = await _userService.GetUserByIdAsync(doctorDto.UserId);
        if (existingUser == null)
            return ServiceResponse.Failed(Resource.NotFound);

        existingUser.FirstName = doctorDto.FirstName;
        existingUser.LastName = doctorDto.LastName;
        existingUser.PhoneNumber = doctorDto.PhoneNumber;

        var updateUserResult = await _userService.UpdateUserAsync(existingUser);
        if (!updateUserResult.Success)
            return ServiceResponse.Failed(updateUserResult.ErrorMessage!);

        var doctor = await _unitOfWork.DoctorRepository.GetByIdAsync(doctorDto.Id);
        if (doctor is null)
            return ServiceResponse.Failed(Resource.NotFound);

        if (doctorDto.SpecialtyId.HasValue && !await _unitOfWork.SpecialtyRepository.ExistsAsync(doctorDto.SpecialtyId.Value))
            return ServiceResponse.Failed(Resource.NotFound);

        doctor.FirstName = doctorDto.FirstName;
        doctor.LastName = doctorDto.LastName;
        doctor.Image = doctorDto.Image;
        doctor.SpecialtyId = doctorDto.SpecialtyId;
        doctor.DepartmentId = doctorDto.DepartmentId;

        await _unitOfWork.DoctorRepository.UpdateAsync(doctor);

        return ServiceResponse.Successful();
    }

    public async Task DeleteDoctorAsync(int id)
    {
        await _unitOfWork.DoctorRepository.DeleteDoctorAsync(id);
    }

    public async Task<IEnumerable<AppointmentDto>> GetDoctorAppointmentsByUserIdAsync(string userId)
    {
        var doctorAppointments = await _unitOfWork.DoctorRepository.GetDoctorAppointmentsByUserIdAsync(userId);
        return doctorAppointments.MapToDto();
    }

    public async Task<ProfileDto?> GetDoctorProfileByUserIdAsync(string userId)
    {
        var doctor = await _unitOfWork.DoctorRepository.GetDoctorByUserIdAsync(userId);

        if (doctor is null) return null;

        var profile = new ProfileDto
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

    public async Task<IEnumerable<PatientDto>> GetMyPatientsAsync(string userId)
    {
        var patients = new List<Patient>();

        var doctor = await _unitOfWork.DoctorRepository.GetDoctorByUserIdAsync(userId);

        if (doctor is null)
            return Enumerable.Empty<PatientDto>();

        var doctorAppointments = await _unitOfWork.AppointmentRepository.GetAllAppointmentsByDoctorIdAsync(doctor.Id);

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

            var treatedPatients = await _unitOfWork.PatientRepository.QueryAsync(queryOptions);
            patients.AddRange(treatedPatients);
        }

        var isSupervisorDoctor = await _unitOfWork.DoctorRepository.IsDoctorHeadOfDepartmentAsync(doctor.Id);

        if (isSupervisorDoctor)
        {
            var departmentId = doctor.DepartmentId ?? 0;
            var departmentPatients = await _unitOfWork.PatientRepository.GetPatientsByDepartmentIdAsync(departmentId);

            patients.AddRange(departmentPatients);
        }

        var distinctPatients = patients
            .GroupBy(x => x.Id)
            .Select(x => x.First())
            .ToList();

        return distinctPatients.MapToDto();
    }

    public Task<int> GetDoctorCountAsync()
    {
        return _unitOfWork.DoctorRepository.GetCountAsync();
    }

    public async Task<IEnumerable<DoctorSimpleDto>> GetAllDoctorsSimpleAsync()
    {
        var doctors = await _unitOfWork.DoctorRepository.GetAllDoctorsSimpleAsync();

        return doctors.MapToSimpleDto();
    }
}

