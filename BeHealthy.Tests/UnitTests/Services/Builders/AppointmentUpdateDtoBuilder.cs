using BeHealthy.Shared.Dtos.Appointment;

namespace BeHealthy.Tests.UnitTests.Services.Builders;

public class AppointmentUpdateDtoBuilder
{
    private readonly IFixture _fixture;
    private int _id = 1;
    private int _doctorId = 1;
    private int _patientId = 1;
    private int? _roomId = 1;
    private int? _nurseId;
    private DateOnly _date = DateOnly.FromDateTime(DateTime.UtcNow);
    private TimeOnly _startTime = TimeOnly.FromDateTime(DateTime.UtcNow);
    private TimeOnly _endTime = TimeOnly.FromDateTime(DateTime.UtcNow.AddHours(1));

    public AppointmentUpdateDtoBuilder(IFixture fixture) { _fixture = fixture; }

    public AppointmentUpdateDtoBuilder WithId(int id) { _id = id; return this; }
    public AppointmentUpdateDtoBuilder WithDoctorId(int id) { _doctorId = id; return this; }
    public AppointmentUpdateDtoBuilder WithPatientId(int id) { _patientId = id; return this; }
    public AppointmentUpdateDtoBuilder WithRoomId(int? id) { _roomId = id; return this; }
    public AppointmentUpdateDtoBuilder WithNurseId(int? id) { _nurseId = id; return this; }
    public AppointmentUpdateDtoBuilder WithDate(DateOnly date) { _date = date; return this; }
    public AppointmentUpdateDtoBuilder WithStartTime(TimeOnly time) { _startTime = time; return this; }
    public AppointmentUpdateDtoBuilder WithEndTime(TimeOnly time) { _endTime = time; return this; }

    public AppointmentUpdateDto Build()
    {
        return _fixture.Build<AppointmentUpdateDto>()
            .With(a => a.DoctorId, _doctorId)
            .With(a => a.PatientId, _patientId)
            .With(a => a.RoomId, _roomId)
            .With(a => a.NurseId, _nurseId)
            .With(a => a.AppointmentDate, _date)
            .With(a => a.AppointmentStartTime, _startTime)
            .With(a => a.AppointmentEndTime, _endTime)
            .Create();
    }
}
