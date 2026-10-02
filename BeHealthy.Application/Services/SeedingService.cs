using BeHealthy.Shared.Dtos.Appointment;
using BeHealthy.Shared.Dtos.Doctor;
using BeHealthy.Shared.Dtos.Nurse;
using BeHealthy.Shared.Dtos.Patient;
using BeHealthy.Application.Interfaces;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Domain;

namespace BeHealthy.Application.Services;

public class SeedingService : ISeedingService
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly INurseRepository _nurseRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IDoctorService _doctorService;
    private readonly IPatientService _patientService;
    private readonly INurseService _nurseService;
    private readonly IAppointmentService _appointmentService;

    public SeedingService(
        IDoctorRepository doctorRepository,
        IPatientRepository patientRepository,
        INurseRepository nurseRepository,
        IAppointmentRepository appointmentRepository,
        IDoctorService doctorService,
        IPatientService patientService,
        INurseService nurseService,
        IAppointmentService appointmentService)
    {
        _doctorRepository = doctorRepository;
        _patientRepository = patientRepository;
        _nurseRepository = nurseRepository;
        _appointmentRepository = appointmentRepository;
        _doctorService = doctorService;
        _patientService = patientService;
        _nurseService = nurseService;
        _appointmentService = appointmentService;
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
            return ServiceResponse.Failed("Count must be between 1 and 10");

        try
        {
            for (int i = 1; i <= count; i++)
            {
                var doctorDto = new DoctorCreateRequest
                {
                    FirstName = $"Doctor{i}",
                    LastName = $"Sample{i}",
                    Email = $"doctor{i}@behealthy.com",
                    Password = "Doctor123!",
                    PhoneNumber = $"1234567{i:D3}",
                    Image = null,
                    DepartmentId = null,
                    SpecialtyId = null
                };

                var result = await _doctorService.AddDoctorAsync(doctorDto, cancellationToken);
                if (!result.Success)
                    return ServiceResponse.Failed($"Failed to create doctor {i}: {result.ErrorMessage}");
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
            return ServiceResponse.Failed("Count must be between 1 and 10");

        try
        {
            for (int i = 1; i <= count; i++)
            {
                var patientDto = new PatientCreateRequest
                {
                    FirstName = $"Patient{i}",
                    LastName = $"Sample{i}",
                    Email = $"patient{i}@behealthy.com",
                    Password = "Patient123!",
                    PhoneNumber = $"1234568{i:D3}",
                    Image = null,
                    DepartmentId = null
                };

                var result = await _patientService.AddPatientAsync(patientDto, cancellationToken);
                if (!result.Success)
                    return ServiceResponse.Failed($"Failed to create patient {i}: {result.ErrorMessage}");
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
            return ServiceResponse.Failed("Count must be between 1 and 10");

        try
        {
            for (int i = 1; i <= count; i++)
            {
                var nurseDto = new NurseCreateRequest
                {
                    FirstName = $"Nurse{i}",
                    LastName = $"Sample{i}",
                    Email = $"nurse{i}@behealthy.com",
                    Password = "Nurse123!",
                    PhoneNumber = $"1234569{i:D3}",
                    Image = null,
                    DepartmentId = null
                };

                var result = await _nurseService.AddNurseAsync(nurseDto, cancellationToken);
                if (!result.Success)
                    return ServiceResponse.Failed($"Failed to create nurse {i}: {result.ErrorMessage}");
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
            return ServiceResponse.Failed("Count must be between 1 and 10");

        try
        {
            var doctors = await _doctorRepository.GetAllDoctorsSimpleAsync(cancellationToken);
            var patients = await _patientRepository.GetAllPatientsSimpleAsync(cancellationToken);

            if (!doctors.Any())
                return ServiceResponse.Failed("No doctors found. Please seed doctors first.");

            if (!patients.Any())
                return ServiceResponse.Failed("No patients found. Please seed patients first.");

            var doctorsList = doctors.ToList();
            var patientsList = patients.ToList();
            var random = new Random();

            for (int i = 1; i <= count; i++)
            {
                var appointmentDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(i));
                var startHour = 9 + (i % 8); // Between 9 AM and 5 PM

                var appointmentDto = new AppointmentCreateRequest
                {
                    DoctorId = doctorsList[random.Next(doctorsList.Count)].Id,
                    PatientId = patientsList[random.Next(patientsList.Count)].Id,
                    AppointmentDate = appointmentDate,
                    AppointmentStartTime = new TimeOnly(startHour, 0),
                    AppointmentEndTime = new TimeOnly(startHour + 1, 0),
                    Reason = (AppointmentReason)(i % Enum.GetValues<AppointmentReason>().Length),
                    Status = AppointmentStatus.Scheduled,
                    Notes = $"Sample appointment {i}",
                    RoomId = null,
                    NurseId = null
                };

                var result = await _appointmentService.AddAppointmentAsync(appointmentDto, cancellationToken);
                if (!result.Success)
                    return ServiceResponse.Failed($"Failed to create appointment {i}: {result.ErrorMessage}");
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
                results.Add($"Doctors: {result.ErrorMessage}");
        }

        if (options.SeedPatients && options.PatientCount > 0)
        {
            var result = await SeedPatientsAsync(options.PatientCount, cancellationToken);
            if (!result.Success)
                results.Add($"Patients: {result.ErrorMessage}");
        }

        if (options.SeedNurses && options.NurseCount > 0)
        {
            var result = await SeedNursesAsync(options.NurseCount, cancellationToken);
            if (!result.Success)
                results.Add($"Nurses: {result.ErrorMessage}");
        }

        if (options.SeedAppointments && options.AppointmentCount > 0)
        {
            var result = await SeedAppointmentsAsync(options.AppointmentCount, cancellationToken);
            if (!result.Success)
                results.Add($"Appointments: {result.ErrorMessage}");
        }

        if (results.Any())
            return ServiceResponse.Failed(string.Join("; ", results));

        return ServiceResponse.Successful();
    }
}