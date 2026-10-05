using BeHealthy.Shared.Dtos.Appointment;
using BeHealthy.Shared.Dtos.Doctor;
using BeHealthy.Shared.Dtos.Nurse;
using BeHealthy.Shared.Dtos.Patient;
using BeHealthy.Application.Interfaces;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Application.Services.Seeding;

namespace BeHealthy.Application.Services;

public class SeedingService : ISeedingService
{
    // A random slot can clash with an existing appointment, so each appointment gets a few tries.
    private const int MaxAppointmentAttempts = 10;

    private readonly IDoctorRepository _doctorRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly INurseRepository _nurseRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ISpecialtyRepository _specialtyRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IDoctorService _doctorService;
    private readonly IPatientService _patientService;
    private readonly INurseService _nurseService;
    private readonly IAppointmentService _appointmentService;
    private readonly SeedDataGenerator _generator;

    public SeedingService(
        IDoctorRepository doctorRepository,
        IPatientRepository patientRepository,
        INurseRepository nurseRepository,
        IAppointmentRepository appointmentRepository,
        IDepartmentRepository departmentRepository,
        ISpecialtyRepository specialtyRepository,
        IRoomRepository roomRepository,
        IDoctorService doctorService,
        IPatientService patientService,
        INurseService nurseService,
        IAppointmentService appointmentService,
        SeedDataGenerator? generator = null)
    {
        _doctorRepository = doctorRepository;
        _patientRepository = patientRepository;
        _nurseRepository = nurseRepository;
        _appointmentRepository = appointmentRepository;
        _departmentRepository = departmentRepository;
        _specialtyRepository = specialtyRepository;
        _roomRepository = roomRepository;
        _doctorService = doctorService;
        _patientService = patientService;
        _nurseService = nurseService;
        _appointmentService = appointmentService;
        _generator = generator ?? new SeedDataGenerator();
    }

    public async Task<Dictionary<string, int>> CheckEntityCountsAsync(CancellationToken cancellationToken = default)
    {
        return new Dictionary<string, int>
        {
            { "Doctors", await _doctorRepository.GetCountAsync(cancellationToken) },
            { "Patients", await _patientRepository.GetCountAsync(cancellationToken) },
            { "Nurses", await _nurseRepository.GetCountAsync(cancellationToken) },
            { "Appointments", await _appointmentRepository.GetCountAsync(cancellationToken) }
        };
    }

    public async Task<bool> NeedsSeedingAsync(CancellationToken cancellationToken = default)
    {
        var counts = await CheckEntityCountsAsync(cancellationToken);
        return counts.Values.All(count => count == 0);
    }

    public async Task<ServiceResponse> SeedDoctorsAsync(int count, CancellationToken cancellationToken = default)
    {
        if (count < 1 || count > 10)
        {
            return ServiceResponse.Failed("Count must be between 1 and 10");
        }

        try
        {
            var departmentIds = await GetDepartmentIdsAsync(cancellationToken);
            var specialtyIds = (await _specialtyRepository.GetAllSpecialtiesAsync(cancellationToken)).Select(s => s.Id).ToList();

            for (int i = 1; i <= count; i++)
            {
                var doctorDto = _generator.Doctor(departmentIds, specialtyIds);

                var result = await _doctorService.AddDoctorAsync(doctorDto, cancellationToken);
                if (!result.Success)
                {
                    return ServiceResponse.Failed($"Failed to create doctor {doctorDto.FirstName} {doctorDto.LastName}: {result.ErrorMessage}");
                }
            }

            return ServiceResponse.Successful();
        }
        catch (Exception ex)
        {
            return ServiceResponse.Failed($"Error seeding doctors: {ex.Message}");
        }
    }

    public async Task<ServiceResponse> SeedPatientsAsync(int count, CancellationToken cancellationToken = default)
    {
        if (count < 1 || count > 10)
        {
            return ServiceResponse.Failed("Count must be between 1 and 10");
        }

        try
        {
            var departmentIds = await GetDepartmentIdsAsync(cancellationToken);

            for (int i = 1; i <= count; i++)
            {
                var patientDto = _generator.Patient(departmentIds);

                var result = await _patientService.AddPatientAsync(patientDto, cancellationToken);
                if (!result.Success)
                {
                    return ServiceResponse.Failed($"Failed to create patient {patientDto.FirstName} {patientDto.LastName}: {result.ErrorMessage}");
                }
            }

            return ServiceResponse.Successful();
        }
        catch (Exception ex)
        {
            return ServiceResponse.Failed($"Error seeding patients: {ex.Message}");
        }
    }

    public async Task<ServiceResponse> SeedNursesAsync(int count, CancellationToken cancellationToken = default)
    {
        if (count < 1 || count > 10)
        {
            return ServiceResponse.Failed("Count must be between 1 and 10");
        }

        try
        {
            var departmentIds = await GetDepartmentIdsAsync(cancellationToken);

            for (int i = 1; i <= count; i++)
            {
                var nurseDto = _generator.Nurse(departmentIds);

                var result = await _nurseService.AddNurseAsync(nurseDto, cancellationToken);
                if (!result.Success)
                {
                    return ServiceResponse.Failed($"Failed to create nurse {nurseDto.FirstName} {nurseDto.LastName}: {result.ErrorMessage}");
                }
            }

            return ServiceResponse.Successful();
        }
        catch (Exception ex)
        {
            return ServiceResponse.Failed($"Error seeding nurses: {ex.Message}");
        }
    }

    public async Task<ServiceResponse> SeedAppointmentsAsync(int count, CancellationToken cancellationToken = default)
    {
        if (count < 1 || count > 10)
        {
            return ServiceResponse.Failed("Count must be between 1 and 10");
        }

        try
        {
            var doctorIds = (await _doctorRepository.GetAllDoctorsSimpleAsync(cancellationToken)).Select(d => d.Id).ToList();
            var patientIds = (await _patientRepository.GetAllPatientsSimpleAsync(cancellationToken)).Select(p => p.Id).ToList();

            if (doctorIds.Count == 0)
            {
                return ServiceResponse.Failed("No doctors found. Please seed doctors first.");
            }

            if (patientIds.Count == 0)
            {
                return ServiceResponse.Failed("No patients found. Please seed patients first.");
            }

            var nurseIds = (await _nurseRepository.GetAllNursesAsync(cancellationToken)).Select(n => n.Id).ToList();
            var roomIds = (await _roomRepository.GetAllRoomsAsync(cancellationToken)).Select(r => r.Id).ToList();

            for (int i = 1; i <= count; i++)
            {
                var result = ServiceResponse.Failed(string.Empty);

                for (int attempt = 1; attempt <= MaxAppointmentAttempts && !result.Success; attempt++)
                {
                    var appointmentDto = _generator.Appointment(doctorIds, patientIds, nurseIds, roomIds);
                    result = await _appointmentService.AddAppointmentAsync(appointmentDto, cancellationToken);
                }

                if (!result.Success)
                {
                    return ServiceResponse.Failed($"Failed to create appointment {i}: {result.ErrorMessage}");
                }
            }

            return ServiceResponse.Successful();
        }
        catch (Exception ex)
        {
            return ServiceResponse.Failed($"Error seeding appointments: {ex.Message}");
        }
    }

    public async Task<ServiceResponse> SeedAllAsync(SeedingOptionsRequest options, CancellationToken cancellationToken = default)
    {
        var results = new List<string>();

        if (options.SeedDoctors && options.DoctorCount > 0)
        {
            var result = await SeedDoctorsAsync(options.DoctorCount, cancellationToken);
            if (!result.Success)
            {
                results.Add($"Doctors: {result.ErrorMessage}");
            }
        }

        if (options.SeedPatients && options.PatientCount > 0)
        {
            var result = await SeedPatientsAsync(options.PatientCount, cancellationToken);
            if (!result.Success)
            {
                results.Add($"Patients: {result.ErrorMessage}");
            }
        }

        if (options.SeedNurses && options.NurseCount > 0)
        {
            var result = await SeedNursesAsync(options.NurseCount, cancellationToken);
            if (!result.Success)
            {
                results.Add($"Nurses: {result.ErrorMessage}");
            }
        }

        if (options.SeedAppointments && options.AppointmentCount > 0)
        {
            var result = await SeedAppointmentsAsync(options.AppointmentCount, cancellationToken);
            if (!result.Success)
            {
                results.Add($"Appointments: {result.ErrorMessage}");
            }
        }

        if (results.Any())
        {
            return ServiceResponse.Failed(string.Join("; ", results));
        }

        return ServiceResponse.Successful();
    }

    private async Task<List<int>> GetDepartmentIdsAsync(CancellationToken cancellationToken)
        => (await _departmentRepository.GetDepartmentsAsync(cancellationToken)).Select(d => d.Id).ToList();
}
