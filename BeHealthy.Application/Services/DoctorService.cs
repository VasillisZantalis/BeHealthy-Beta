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

    public async Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync()
    {
        var doctors = await _unitOfWork.DoctorRepository.GetAllDoctorsAsync();
        return doctors.MapToDto();
    }

    public async Task<DoctorDto?> GetDoctorByIdAsync(int id)
    {
        var doctor = await _unitOfWork.DoctorRepository.GetByIdAsync(id);
        return doctor?.MapToDto();
    }

    public async Task<ServiceResponse> AddDoctorAsync(DoctorForCreationDto doctorDto)
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
            await _userService.CreateApplicationUser(user, doctorDto.Password);
            await _userService.AddUserToRoleAsync(user, UserRole.Doctor);
            
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

    public async Task UpdateDoctorAsync(int id, DoctorForUpdateDto doctorDto)
    {
        var doctor = doctorDto?.MapToDomain();

        if (doctor is null || !await _unitOfWork.DoctorRepository.ExistsAsync(id))
            return;

        if (doctor.SpecialtyId.HasValue && !await _unitOfWork.SpecialtyRepository.ExistsAsync(doctor.SpecialtyId.Value))
            return;

        await _unitOfWork.DoctorRepository.UpdateAsync(doctor);
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
            var treatedPatients = await _unitOfWork.PatientRepository.FindWithIncludesAsync(
                w => patientIds.Contains(w.Id),
                false,
                w => w.User);
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
}

