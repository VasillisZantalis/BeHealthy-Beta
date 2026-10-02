using BeHealthy.Application.Interfaces;
using BeHealthy.Application.Interfaces.Repositories;
using BeHealthy.Tests.UnitTests.Services.Builders;

namespace BeHealthy.Tests.UnitTests.Services;

public class AppointmentServiceTests
{
    private readonly Mock<IAppointmentRepository> _mockAppointmentRepository;
    private readonly Mock<IDoctorRepository> _mockDoctorRepository;
    private readonly Mock<IPatientRepository> _mockPatientRepository;
    private readonly Mock<IRoomRepository> _mockRoomRepository;
    private readonly AppointmentService _sut;
    private readonly IFixture _fixture;

    public AppointmentServiceTests()
    {
        _fixture = new Fixture();
        _fixture.Customize<DateOnly>(o => o.FromFactory((DateTime dt) => DateOnly.FromDateTime(dt)));
        _fixture.Customize<TimeOnly>(o => o.FromFactory((DateTime dt) => TimeOnly.FromDateTime(dt)));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));

        _mockAppointmentRepository = new Mock<IAppointmentRepository>();
        _mockDoctorRepository = new Mock<IDoctorRepository>();
        _mockPatientRepository = new Mock<IPatientRepository>();
        _mockRoomRepository = new Mock<IRoomRepository>();

        _sut = new AppointmentService(
            _mockAppointmentRepository.Object,
            _mockDoctorRepository.Object,
            _mockPatientRepository.Object,
            _mockRoomRepository.Object);
    }

    #region GetAllAppointmentsByDoctorIdAsync

    [Fact]
    public async Task GetAllAppointmentsByDoctorIdAsync_WithValidDoctorId_ReturnsAppointments()
    {
        //Arrange
        var appointments = new AppointmentBuilder(_fixture)
            .WithDoctorId(1)
            .BuildMany(2);

        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsByDoctorIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(appointments);

        //Act
        var result = await _sut.GetAllAppointmentsByDoctorIdAsync(1);

        //Assert
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetAllAppointmentsByDoctorIdAsync_WithInvalidDoctorId_ReturnsEmptyList()
    {
        //Arrange
        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsByDoctorIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new List<Appointment>());

        //Act
        var result = await _sut.GetAllAppointmentsByDoctorIdAsync(-1);

        //Assert
        result.ShouldBeEmpty();
    }

    #endregion

    #region GetAllAppointmentsAsync

    [Fact]
    public async Task GetAllAppointmentsAsync_WithAppointments_ReturnsAppointments()
    {
        //Arrange
        IEnumerable<Appointment> appointments = new AppointmentBuilder(_fixture)
            .BuildMany(2);

        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(appointments);

        //Act
        var result = await _sut.GetAllAppointmentsAsync();

        //Assert
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetAllAppointmentsAsync_NoAppointments_ReturnsEmptyList()
    {
        //Arrange
        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Appointment>());

        //Act
        var result = await _sut.GetAllAppointmentsAsync();

        //Asser
        result.Items.ShouldBeEmpty();
    }

    #endregion

    #region GetAllAppointmentsByPatientIdAsync

    [Fact]
    public async Task GetAllAppointmentsByPatientIdAsync_WithValidPatientId_ReturnsAppointments()
    {
        //Arrange
        IEnumerable<Appointment> appointments = new AppointmentBuilder(_fixture)
            .WithPatientId(1)
            .BuildMany(2);

        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsByPatientIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(appointments);

        //Act
        var result = await _sut.GetAllAppointmentsByPatientIdAsync(1);

        //Assert
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetAllAppointmentsByPatientIdAsync_WithInvalidPatientId_ReturnsEmptyList()
    {
        //Arrange
        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsByPatientIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Appointment>());

        //Act
        var result = await _sut.GetAllAppointmentsByPatientIdAsync(-1);

        //Assert
        result.ShouldBeEmpty();
    }

    #endregion

    #region GetAppointmentByIdAsync

    [Fact]
    public async Task GetAppointmentByIdAsync_WithValidId_ReturnsAppointment()
    {
        //Arrange
        var appointment = new AppointmentBuilder(_fixture).Build();

        _mockAppointmentRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(appointment);

        //Act
        var result = await _sut.GetAppointmentByIdAsync(1);

        //Assert
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetAppointmentByIdAsync_WithInvalidId_ReturnsNull()
    {
        //Arrange
        _mockAppointmentRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Appointment?)null);

        //Act
        var result = await _sut.GetAppointmentByIdAsync(-1);

        //Assert
        result.ShouldBeNull();
    }

    #endregion

    #region AddAppointmentAsync

    [Fact]
    public async Task AddAppointment_ValidAppointment_CreatesAppointment()
    {
        //Arrange
        var appointment = new AppointmentCreateDtoBuilder(_fixture)
            .WithRoomId(null)
            .Build();

        _mockAppointmentRepository.Setup(r => r.AddAsync(It.IsAny<Appointment>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsByDoctorIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new List<Appointment>());

        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsByPatientIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync(new List<Appointment>());
        _mockDoctorRepository.Setup(r => r.ExistsAsync(appointment.DoctorId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _mockPatientRepository.Setup(r => r.ExistsAsync(appointment.PatientId, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        //Act
        var result = await _sut.AddAppointmentAsync(appointment);

        //Assert
        result.Success.ShouldBeTrue();
    }

    [Fact]
    public async Task AddAppointment_NullAppointment_ReturnsFailedResponse()
    {
        //Arrange
        _mockAppointmentRepository.Setup(r => r.AddAsync(It.IsAny<Appointment>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        //Act
        var result = await _sut.AddAppointmentAsync(null!);

        //Assert
        result.Success.ShouldBeFalse();
    }

    [Fact]
    public async Task AddAppointment_InvalidDoctorId_ReturnsFailedResponse()
    {
        //Arrange
        var appointment = new AppointmentCreateDtoBuilder(_fixture)
            .WithDoctorId(-1)
            .Build();

        _mockAppointmentRepository.Setup(r => r.AddAsync(It.IsAny<Appointment>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockDoctorRepository.Setup(r => r.ExistsAsync(appointment.DoctorId, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        //Act
        var result = await _sut.AddAppointmentAsync(appointment);

        //Assert
        result.Success.ShouldBeFalse();
    }

    [Fact]
    public async Task AddAppointment_InvalidPatientId_ReturnsFailedResponse()
    {
        //Arrange
        var appointment = new AppointmentCreateDtoBuilder(_fixture)
            .WithPatientId(-1)
            .Build();

        _mockAppointmentRepository.Setup(r => r.AddAsync(It.IsAny<Appointment>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockDoctorRepository.Setup(r => r.ExistsAsync(appointment.DoctorId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _mockPatientRepository.Setup(r => r.ExistsAsync(appointment.PatientId, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        //Act
        var result = await _sut.AddAppointmentAsync(appointment);

        //Assert
        result.Success.ShouldBeFalse();
    }

    [Fact]
    public async Task AddAppointment_InvalidRoomId_ReturnsFailedResponse()
    {
        //Arrange
        var appointment = new AppointmentCreateDtoBuilder(_fixture)
            .WithRoomId(-1)
            .Build();

        _mockAppointmentRepository.Setup(r => r.AddAsync(It.IsAny<Appointment>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockDoctorRepository.Setup(r => r.ExistsAsync(appointment.DoctorId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _mockPatientRepository.Setup(r => r.ExistsAsync(appointment.PatientId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _mockRoomRepository.Setup(r => r.ExistsAsync(appointment.RoomId!.Value, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        //Act
        var result = await _sut.AddAppointmentAsync(appointment);

        //Assert
        result.Success.ShouldBeFalse();
    }

    [Fact]
    public async Task AddAppointment_WithPatientConflict_ReturnsFailedResponse()
    {
        //Arrange
        var appointmentDate = DateOnly.FromDateTime(DateTime.Today);
        var startTime = new TimeOnly(10, 0);
        var endTime = new TimeOnly(11, 0);

        var appointment = new AppointmentCreateDtoBuilder(_fixture)
            .WithDate(appointmentDate)
            .WithStartTime(startTime)
            .WithEndTime(endTime)
            .Build();

        var existingAppointments = new AppointmentBuilder(_fixture)
            .WithDate(appointmentDate)
            .WithStartTime(startTime)
            .WithEndTime(endTime)
            .BuildMany(1);

        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsByDoctorIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new List<Appointment>());

        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsByPatientIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync(existingAppointments);
        _mockDoctorRepository.Setup(r => r.ExistsAsync(appointment.DoctorId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _mockPatientRepository.Setup(r => r.ExistsAsync(appointment.PatientId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _mockRoomRepository.Setup(r => r.ExistsAsync(appointment.RoomId!.Value, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        //Act
        var result = await _sut.AddAppointmentAsync(appointment);

        //Assert
        result.Success.ShouldBeFalse();
    }

    [Fact]
    public async Task AddAppointment_WithDoctorConflict_ReturnsFailedResponse()
    {
        //Arrange
        var appointmentDate = DateOnly.FromDateTime(DateTime.Today);
        var startTime = new TimeOnly(10, 0);
        var endTime = new TimeOnly(11, 0);

        var appointment = new AppointmentCreateDtoBuilder(_fixture)
            .WithDate(appointmentDate)
            .WithStartTime(startTime)
            .WithEndTime(endTime)
            .Build();

        var existingAppointments = new AppointmentBuilder(_fixture)
            .WithDate(appointmentDate)
            .WithStartTime(startTime)
            .WithEndTime(endTime)
            .BuildMany(1);

        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsByDoctorIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(existingAppointments);
        _mockDoctorRepository.Setup(r => r.ExistsAsync(appointment.DoctorId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _mockPatientRepository.Setup(r => r.ExistsAsync(appointment.PatientId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _mockRoomRepository.Setup(r => r.ExistsAsync(appointment.RoomId!.Value, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        //Act
        var result = await _sut.AddAppointmentAsync(appointment);

        //Assert
        result.Success.ShouldBeFalse();
    }

    [Fact]
    public async Task AddAppointment_WithRoomConflicts_ReturnsFailedResponse()
    {
        //Arrange
        var appointmentDate = DateOnly.FromDateTime(DateTime.Today);
        var startTime = new TimeOnly(10, 0);
        var endTime = new TimeOnly(11, 0);

        var appointment = new AppointmentCreateDtoBuilder(_fixture)
            .WithDate(appointmentDate)
            .WithStartTime(startTime)
            .WithEndTime(endTime)
            .WithRoomId(1)
            .Build();

        var existingAppointments = new AppointmentBuilder(_fixture)
            .WithDate(appointmentDate)
            .WithStartTime(startTime)
            .WithEndTime(endTime)
            .WithRoomId(1)
            .BuildMany(1);

        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsByDoctorIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new List<Appointment>());

        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsByPatientIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync(new List<Appointment>());

        _mockRoomRepository.Setup(r => r.GetRoomAppointmentsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingAppointments.ToList());

        _mockDoctorRepository.Setup(r => r.ExistsAsync(appointment.DoctorId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _mockPatientRepository.Setup(r => r.ExistsAsync(appointment.PatientId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _mockRoomRepository.Setup(r => r.ExistsAsync(appointment.RoomId!.Value, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        //Act
        var result = await _sut.AddAppointmentAsync(appointment);

        //Assert
        result.Success.ShouldBeFalse();
    }

    #endregion

    #region UpdateAppointmentAsync

    [Fact]
    public async Task UpdateAppointmentAsync_ValidAppointment_UpdateAppointment()
    {
        //Arrange
        var appointment = new AppointmentUpdateDtoBuilder(_fixture)
            .WithRoomId(null)
            .Build();

        _mockAppointmentRepository.Setup(r => r.GetByIdAsync(appointment.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Appointment { Id = appointment.Id });

        _mockAppointmentRepository.Setup(r => r.UpdateAsync(It.IsAny<Appointment>()))
            .Returns(Task.CompletedTask);

        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsByDoctorIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new List<Appointment>());

        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsByPatientIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync(new List<Appointment>());

        _mockDoctorRepository.Setup(r => r.ExistsAsync(appointment.DoctorId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _mockPatientRepository.Setup(r => r.ExistsAsync(appointment.PatientId, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        //Act
        var result = await _sut.UpdateAppointmentAsync(appointment);

        //Assert
        result.Success.ShouldBeTrue();
        _mockAppointmentRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAppointmentAsync_NullAppointment_ReturnsFailedResponse()
    {
        //Arrange
        _mockAppointmentRepository.Setup(r => r.UpdateAsync(It.IsAny<Appointment>()))
            .Returns(Task.CompletedTask);

        //Act
        var result = await _sut.UpdateAppointmentAsync(null!);

        //Assert
        result.Success.ShouldBeFalse();
    }

    [Fact]
    public async Task UpdateAppointmentAsync_InvalidDoctorId_ReturnsFailedResponse()
    {
        //Arrange
        var appointment = new AppointmentUpdateDtoBuilder(_fixture)
            .WithDoctorId(-1)
            .Build();

        _mockAppointmentRepository.Setup(r => r.UpdateAsync(It.IsAny<Appointment>()))
            .Returns(Task.CompletedTask);

        _mockDoctorRepository.Setup(r => r.ExistsAsync(appointment.DoctorId, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        //Act
        var result = await _sut.UpdateAppointmentAsync(appointment);

        //Assert
        result.Success.ShouldBeFalse();
    }

    [Fact]
    public async Task UpdateAppointmentAsync_InvalidPatientId_ReturnsFailedResponse()
    {
        //Arrange
        var appointment = new AppointmentUpdateDtoBuilder(_fixture)
            .WithPatientId(-1)
            .Build();

        _mockAppointmentRepository.Setup(r => r.UpdateAsync(It.IsAny<Appointment>()))
            .Returns(Task.CompletedTask);
        _mockDoctorRepository.Setup(r => r.ExistsAsync(appointment.DoctorId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _mockPatientRepository.Setup(r => r.ExistsAsync(appointment.PatientId, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        //Act
        var result = await _sut.UpdateAppointmentAsync(appointment);

        //Assert
        result.Success.ShouldBeFalse();
    }

    [Fact]
    public async Task UpdateAppointmentAsync_InvalidRoomId_ReturnsFailedResponse()
    {
        //Arrange
        var appointment = new AppointmentUpdateDtoBuilder(_fixture)
            .WithRoomId(-1)
            .Build();

        _mockAppointmentRepository.Setup(r => r.AddAsync(It.IsAny<Appointment>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockDoctorRepository.Setup(r => r.ExistsAsync(appointment.DoctorId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _mockPatientRepository.Setup(r => r.ExistsAsync(appointment.PatientId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _mockRoomRepository.Setup(r => r.ExistsAsync(appointment.RoomId!.Value, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        //Act
        var result = await _sut.UpdateAppointmentAsync(appointment);

        //Assert
        result.Success.ShouldBeFalse();
    }

    [Fact]
    public async Task UpdateAppointmentAsync_WithPatientConflict_ReturnsFailedResponse()
    {
        //Arrange
        var appointmentDate = DateOnly.FromDateTime(DateTime.Today);
        var startTime = new TimeOnly(10, 0);
        var endTime = new TimeOnly(11, 0);

        var appointment = new AppointmentUpdateDtoBuilder(_fixture)
            .WithDate(appointmentDate)
            .WithStartTime(startTime)
            .WithEndTime(endTime)
            .Build();

        var existingAppointments = new AppointmentBuilder(_fixture)
            .WithDate(appointmentDate)
            .WithStartTime(startTime)
            .WithEndTime(endTime)
            .BuildMany(1);

        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsByDoctorIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new List<Appointment>());

        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsByPatientIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync(existingAppointments);
        _mockDoctorRepository.Setup(r => r.ExistsAsync(appointment.DoctorId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _mockPatientRepository.Setup(r => r.ExistsAsync(appointment.PatientId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _mockRoomRepository.Setup(r => r.ExistsAsync(appointment.RoomId!.Value, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        //Act
        var result = await _sut.UpdateAppointmentAsync(appointment);

        //Assert
        result.Success.ShouldBeFalse();
    }

    [Fact]
    public async Task UpdateAppointmentAsync_WithDoctorConflict_ReturnsFailedResponse()
    {
        //Arrange
        var appointmentDate = DateOnly.FromDateTime(DateTime.Today);
        var startTime = new TimeOnly(10, 0);
        var endTime = new TimeOnly(11, 0);

        var appointment = new AppointmentUpdateDtoBuilder(_fixture)
            .WithDate(appointmentDate)
            .WithStartTime(startTime)
            .WithEndTime(endTime)
            .Build();

        var existingAppointments = new AppointmentBuilder(_fixture)
            .WithDate(appointmentDate)
            .WithStartTime(startTime)
            .WithEndTime(endTime)
            .BuildMany(1);

        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsByDoctorIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(existingAppointments);
        _mockDoctorRepository.Setup(r => r.ExistsAsync(appointment.DoctorId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _mockPatientRepository.Setup(r => r.ExistsAsync(appointment.PatientId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _mockRoomRepository.Setup(r => r.ExistsAsync(appointment.RoomId!.Value, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        //Act
        var result = await _sut.UpdateAppointmentAsync(appointment);

        //Assert
        result.Success.ShouldBeFalse();
    }

    [Fact]
    public async Task UpdateAppointmentAsync_WithRoomConflicts_ReturnsFailedResponse()
    {
        //Arrange
        var appointmentDate = DateOnly.FromDateTime(DateTime.Today);
        var startTime = new TimeOnly(10, 0);
        var endTime = new TimeOnly(11, 0);

        var appointment = new AppointmentUpdateDtoBuilder(_fixture)
            .WithDate(appointmentDate)
            .WithStartTime(startTime)
            .WithEndTime(endTime)
            .WithRoomId(1)
            .Build();

        var existingAppointments = new AppointmentBuilder(_fixture)
            .WithDate(appointmentDate)
            .WithStartTime(startTime)
            .WithEndTime(endTime)
            .WithRoomId(1)
            .BuildMany(1);

        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsByDoctorIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new List<Appointment>());

        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsByPatientIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync(new List<Appointment>());

        _mockRoomRepository.Setup(r => r.GetRoomAppointmentsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingAppointments.ToList());

        _mockDoctorRepository.Setup(r => r.ExistsAsync(appointment.DoctorId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _mockPatientRepository.Setup(r => r.ExistsAsync(appointment.PatientId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _mockRoomRepository.Setup(r => r.ExistsAsync(appointment.RoomId!.Value, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        //Act
        var result = await _sut.UpdateAppointmentAsync(appointment);

        //Assert
        result.Success.ShouldBeFalse();
    }

    #endregion

    #region GetAllAppointmentsByUserIdAsync

    [Fact]
    public async Task GetAllAppointmentsByUserIdAsync_WithValidUserId_ReturnsAppointments()
    {
        //Arrange
        var appointments = new AppointmentBuilder(_fixture).BuildMany(2);

        var userId = Guid.NewGuid().ToString();

        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(appointments);
        //Act
        var result = await _sut.GetAllAppointmentsByUserIdAsync(userId);

        //Assert
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetAllAppointmentsByUserIdAsync_InvalidUserId_ReturnsEmptyList()
    {
        //Arrange
        var userId = string.Empty;

        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new List<Appointment>());
        //Act
        var result = await _sut.GetAllAppointmentsByUserIdAsync(userId);

        //Assert
        result.ShouldBeEmpty();
    }

    [Fact]
    public async Task GetAllAppointmentsByUserIdAsync_NullUserId_ReturnsEmptyList()
    {
        //Arrange
        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new List<Appointment>());
        //Act
        var result = await _sut.GetAllAppointmentsByUserIdAsync(null!);

        //Assert
        result.ShouldBeEmpty();
    }

    #endregion

    #region DeleteAppointmentAsync

    [Fact]
    public async Task DeleteAppointmentAsync_ValidId_DeletesAndSaves()
    {
        //Arrange
        var appointmentId = 1;
        _mockAppointmentRepository.Setup(r => r.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        //Act
        await _sut.DeleteAppointmentAsync(appointmentId);

        //Assert
        _mockAppointmentRepository.Verify(r => r.DeleteAsync(appointmentId, It.IsAny<CancellationToken>()), Times.Once);
        _mockAppointmentRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAppointmentAsync_NotFound_DoesNotSave()
    {
        //Arrange
        _mockAppointmentRepository.Setup(r => r.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        //Act
        await _sut.DeleteAppointmentAsync(1);

        //Assert
        _mockAppointmentRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAppointmentAsync_WhenRepositoryThrows_ThrowsException()
    {
        // Arrange
        var id = 10;

        _mockAppointmentRepository
            .Setup(r => r.DeleteAsync(id, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        // Act & Assert
        await Should.ThrowAsync<Exception>(() => _sut.DeleteAppointmentAsync(id));

    }

    #endregion

    #region GetAppointmentReasonCounts

    #endregion
}
