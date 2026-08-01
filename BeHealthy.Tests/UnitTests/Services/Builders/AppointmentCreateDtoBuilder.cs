using BeHealthy.Shared.Dtos.Appointment;

namespace BeHealthy.Tests.UnitTests.Services.Builders;

public class AppointmentCreateDtoBuilder
{
    private readonly IFixture _fixture;
    private int _doctorId = 1;
    private int _patientId = 1;
    private int? _roomId = 1;
    private int? _nurseId;
    private DateOnly _date = DateOnly.FromDateTime(DateTime.UtcNow);
    private TimeOnly _startTime = TimeOnly.FromDateTime(DateTime.UtcNow);
    private TimeOnly _endTime = TimeOnly.FromDateTime(DateTime.UtcNow.AddHours(1));

    public AppointmentCreateDtoBuilder(IFixture fixture) { _fixture = fixture; }

    public AppointmentCreateDtoBuilder WithDoctorId(int id) { _doctorId = id; return this; }
    public AppointmentCreateDtoBuilder WithPatientId(int id) { _patientId = id; return this; }
    public AppointmentCreateDtoBuilder WithRoomId(int? id) { _roomId = id; return this; }
    public AppointmentCreateDtoBuilder WithNurseId(int? id) { _nurseId = id; return this; }
    public AppointmentCreateDtoBuilder WithDate(DateOnly date) { _date = date; return this; }
    public AppointmentCreateDtoBuilder WithStartTime(TimeOnly time) { _startTime = time; return this; }
    public AppointmentCreateDtoBuilder WithEndTime(TimeOnly time) { _endTime = time; return this; }

    public AppointmentCreateDto Build()
    {
        return _fixture.Build<AppointmentCreateDto>()
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
