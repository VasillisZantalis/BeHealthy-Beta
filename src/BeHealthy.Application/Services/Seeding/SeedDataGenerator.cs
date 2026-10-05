using Bogus;

namespace BeHealthy.Application.Services.Seeding;

/// <summary>
/// Builds realistic sample data (names, emails, phone numbers, appointment slots and notes) with Bogus.
/// It only builds requests; <see cref="SeedingService"/> sends them through the normal services.
/// </summary>
public class SeedDataGenerator
{
    public const string EmailDomain = "behealthy.com";
    public const string DoctorPassword = "Doctor123!";
    public const string PatientPassword = "Patient123!";
    public const string NursePassword = "Nurse123!";

    private const int AppointmentDaysRange = 30;
    private const int FirstSlotHour = 8;
    private const int SlotCount = 18; // 30-minute slots from 08:00 to 16:30

    private static readonly Dictionary<AppointmentReason, string[]> NotesByReason = new()
    {
        [AppointmentReason.GeneralCheckup] =
        [
            "Annual physical examination.",
            "Routine blood pressure and cholesterol check.",
            "Pre-employment health screening.",
            "General wellness visit, no current complaints."
        ],
        [AppointmentReason.FollowUp] =
        [
            "Follow-up on recent blood test results.",
            "Post-operative wound check.",
            "Review response to the new medication.",
            "Follow-up after hospital discharge."
        ],
        [AppointmentReason.Illness] =
        [
            "Persistent cough and mild fever for three days.",
            "Recurring migraines, worse in the mornings.",
            "Sore throat and difficulty swallowing.",
            "Stomach pain and nausea after meals."
        ],
        [AppointmentReason.Injury] =
        [
            "Sprained ankle while running.",
            "Lower back pain after lifting a heavy box.",
            "Minor burn on the left hand.",
            "Wrist pain after a fall."
        ],
        [AppointmentReason.Prescription] =
        [
            "Renewal of blood pressure medication.",
            "Repeat prescription for asthma inhaler.",
            "Adjust dosage of thyroid medication.",
            "Renewal of allergy medication before the season starts."
        ]
    };

    private readonly Faker _faker;

    public SeedDataGenerator(int? seed = null)
    {
        _faker = new Faker("en");
        if (seed.HasValue)
        {
            _faker.Random = new Randomizer(seed.Value);
        }
    }

    public DoctorCreateRequest Doctor(IReadOnlyList<int> departmentIds, IReadOnlyList<int> specialtyIds)
    {
        var (firstName, lastName) = Name();

        return new DoctorCreateRequest
        {
            FirstName = firstName,
            LastName = lastName,
            Email = EmailFor(firstName, lastName),
            Password = DoctorPassword,
            ConfirmPassword = DoctorPassword,
            PhoneNumber = PhoneNumber(),
            DepartmentId = PickOrNull(departmentIds),
            SpecialtyId = PickOrNull(specialtyIds)
        };
    }

    public PatientCreateRequest Patient(IReadOnlyList<int> departmentIds)
    {
        var (firstName, lastName) = Name();

        return new PatientCreateRequest
        {
            FirstName = firstName,
            LastName = lastName,
            Email = EmailFor(firstName, lastName),
            Password = PatientPassword,
            ConfirmPassword = PatientPassword,
            PhoneNumber = PhoneNumber(),
            DepartmentId = PickOrNull(departmentIds)
        };
    }

    public NurseCreateRequest Nurse(IReadOnlyList<int> departmentIds)
    {
        var (firstName, lastName) = Name();

        return new NurseCreateRequest
        {
            FirstName = firstName,
            LastName = lastName,
            Email = EmailFor(firstName, lastName),
            Password = NursePassword,
            ConfirmPassword = NursePassword,
            PhoneNumber = PhoneNumber(),
            DepartmentId = PickOrNull(departmentIds)
        };
    }

    /// <summary>
    /// A weekday appointment within a month either side of today. Past appointments are mostly
    /// completed, upcoming ones mostly scheduled.
    /// </summary>
    public AppointmentCreateRequest Appointment(
        IReadOnlyList<int> doctorIds,
        IReadOnlyList<int> patientIds,
        IReadOnlyList<int> nurseIds,
        IReadOnlyList<int> roomIds)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var date = WeekdayNear(today);
        var start = new TimeOnly(FirstSlotHour, 0).AddMinutes(30 * _faker.Random.Int(0, SlotCount - 1));
        var reason = _faker.PickRandom<AppointmentReason>();

        return new AppointmentCreateRequest
        {
            DoctorId = Pick(doctorIds),
            PatientId = Pick(patientIds),
            NurseId = _faker.Random.Bool() ? PickOrNull(nurseIds) : null,
            RoomId = PickOrNull(roomIds),
            AppointmentDate = date,
            AppointmentStartTime = start,
            AppointmentEndTime = start.AddMinutes(_faker.PickRandom(30, 60)),
            Reason = reason,
            Status = StatusFor(date, today),
            Notes = _faker.PickRandom(NotesByReason[reason])
        };
    }

    private (string FirstName, string LastName) Name()
        => (_faker.Name.FirstName(), _faker.Name.LastName());

    private string EmailFor(string firstName, string lastName)
        => _faker.Internet.Email(firstName, lastName, EmailDomain, _faker.Random.Int(10, 9999).ToString()).ToLowerInvariant();

    // E.164, as the PhoneNumber validation rule expects: +1, a 3-digit area code, then 7 digits.
    private string PhoneNumber()
        => $"+1{_faker.Random.Int(201, 989)}{_faker.Random.Int(2000000, 9999999)}";

    private int? PickOrNull(IReadOnlyList<int> ids)
        => ids.Count == 0 ? null : Pick(ids);

    private int Pick(IReadOnlyList<int> ids)
        => ids[_faker.Random.Int(0, ids.Count - 1)];

    private DateOnly WeekdayNear(DateOnly today)
    {
        var date = today.AddDays(_faker.Random.Int(-AppointmentDaysRange, AppointmentDaysRange));

        return date.DayOfWeek switch
        {
            DayOfWeek.Saturday => date.AddDays(2),
            DayOfWeek.Sunday => date.AddDays(1),
            _ => date
        };
    }

    private AppointmentStatus StatusFor(DateOnly date, DateOnly today)
    {
        if (date < today)
        {
            return _faker.Random.WeightedRandom([AppointmentStatus.Completed, AppointmentStatus.Cancelled], [0.85f, 0.15f]);
        }

        return _faker.Random.WeightedRandom([AppointmentStatus.Scheduled, AppointmentStatus.Rescheduled], [0.9f, 0.1f]);
    }
}
