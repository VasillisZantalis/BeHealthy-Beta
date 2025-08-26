namespace BeHealthy.Tests.UnitTests.Services.Builders;

public class AppointmentBuilder
{
    private readonly IFixture _fixture;
    private int _id = 1;
    private int _doctorId = 1;
    private int _patientId = 1;
    private int? _roomId;
    private int? _nurseId;
    private DateOnly _date = DateOnly.FromDateTime(DateTime.UtcNow);
    private TimeOnly _startTime = TimeOnly.FromDateTime(DateTime.UtcNow);
    private TimeOnly _endTime = TimeOnly.FromDateTime(DateTime.UtcNow.AddHours(1));
    
    public AppointmentBuilder(IFixture fixture) { _fixture = fixture; }
    
    public AppointmentBuilder WithId(int id) { _id = id; return this; }
    public AppointmentBuilder WithDoctorId(int doctorId) { _doctorId = doctorId; return this; }
    public AppointmentBuilder WithPatientId(int patientId) { _patientId = patientId; return this; }
    public AppointmentBuilder WithRoomId(int roomId) { _roomId = roomId; return this; }
    public AppointmentBuilder WithNurseId(int nurseId) { _nurseId = nurseId; return this; }
    public AppointmentBuilder WithDate(DateOnly date) { _date = date; return this; }
    public AppointmentBuilder WithStartTime(TimeOnly startTime) { _startTime = startTime; return this; }
    public AppointmentBuilder WithEndTime(TimeOnly endTime) { _endTime = endTime; return this; }

    public Appointment Build()
    {
        return _fixture.Build<Appointment>()
            .Without(a => a.Doctor)
            .Without(a => a.Patient)
            .Without(a => a.Room)
            .Without(a => a.Nurse)
            .With(a => a.Id, _id)
            .With(a => a.DoctorId, _doctorId)
            .With(a => a.PatientId, _patientId)
            .With(a => a.RoomId, _roomId)
            .With(a => a.NurseId, _nurseId)
            .With(a => a.AppointmentDate, _date)
            .With(a => a.AppointmentStartTime, _startTime)
            .With(a => a.AppointmentEndTime, _endTime)
            .Create();
    }

    public IEnumerable<Appointment> BuildMany(int count)
    {
        return Enumerable.Range(0, count)
            .Select(_ => Build())
            .ToList();
    }
}
