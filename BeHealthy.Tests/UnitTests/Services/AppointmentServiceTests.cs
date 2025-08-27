using AutoFixture;
using BeHealthy.Application.Dtos.Appointment;
using BeHealthy.Tests.UnitTests.Services.Builders;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BeHealthy.Tests.UnitTests.Services;

public class AppointmentServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IAppointmentRepository> _mockAppointmentRepository;
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

        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockAppointmentRepository = new Mock<IAppointmentRepository>();

        _mockUnitOfWork.Setup(uow => uow.AppointmentRepository).Returns(_mockAppointmentRepository.Object);

        _sut = new AppointmentService(_mockUnitOfWork.Object);
    }

    #region GetAllAppointmentsByDoctorIdAsync

    [Fact]
    public async Task GetAllAppointmentsByDoctorIdAsync_WithValidDoctorId_ReturnsAppointments()
    {
        //Arrange
        var appointments = new AppointmentBuilder(_fixture)
            .WithDoctorId(1)
            .BuildMany(2);

        _mockAppointmentRepository
            .Setup(r => r.GetAllAppointmentsByDoctorIdAsync(It.IsAny<int>()))
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
        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsByDoctorIdAsync(It.IsAny<int>()))
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

        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsAsync())
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
        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsAsync())
            .ReturnsAsync(new List<Appointment>());

        //Act
        var result = await _sut.GetAllAppointmentsAsync();

        //Asser
        result.ShouldBeEmpty();
    }

    #endregion

    #region GetAllAppointmentsByPatientIdAsync

    [Fact]
    public async Task GetAllAppointmentsByPatientIdAsync_WithValidPatientId_ReturnsAppointments()
    {
        //Arrange
        IEnumerable<Appointment> appointments =  new AppointmentBuilder(_fixture)
            .WithPatientId(1)
            .BuildMany(2);

        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsByPatientIdAsync(It.IsAny<int>()))
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
        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsByPatientIdAsync(It.IsAny<int>()))
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

        _mockAppointmentRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
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
        _mockAppointmentRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
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

        _mockUnitOfWork.Setup(uow => uow.AppointmentRepository.AddAsync(It.IsAny<Appointment>()))
            .Returns(Task.CompletedTask);
        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsByDoctorIdAsync(It.IsAny<int>())).ReturnsAsync([]);
        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsByPatientIdAsync(It.IsAny<int>())).ReturnsAsync([]);
        _mockUnitOfWork.Setup(u => u.DoctorRepository.ExistsAsync(appointment.DoctorId)).ReturnsAsync(true);
        _mockUnitOfWork.Setup(u => u.PatientRepository.ExistsAsync(appointment.PatientId)).ReturnsAsync(true);

        //Act
        var result = await _sut.AddAppointmentAsync(appointment);

        //Assert
        result.Success.ShouldBeTrue();
    }

    [Fact]
    public async Task AddAppontment_InvalidAppointment_ReturnsFailedResponse()
    {
        //Arrange

        //Act

        //Assert
    }

    [Fact]
    public async Task AddAppointment_NullAppointment_ThrowsNullException()
    {
        //Arrange

        //Act

        //Assert
    }

    [Fact]
    public async Task AddAppointment_InvalidDoctorId_ReturnsFailedResponse()
    {
        //Arrange
        var appointment = new AppointmentCreateDtoBuilder(_fixture)
            .WithDoctorId(-1)
            .Build();

        _mockAppointmentRepository.Setup(r => r.AddAsync(It.IsAny<Appointment>()))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(u => u.DoctorRepository.ExistsAsync(appointment.DoctorId)).ReturnsAsync(false);

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

        _mockAppointmentRepository.Setup(r => r.AddAsync(It.IsAny<Appointment>()))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(u => u.DoctorRepository.ExistsAsync(appointment.DoctorId)).ReturnsAsync(true);
        _mockUnitOfWork.Setup(u => u.PatientRepository.ExistsAsync(appointment.PatientId)).ReturnsAsync(false);

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

        _mockAppointmentRepository.Setup(r => r.AddAsync(It.IsAny<Appointment>()))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(u => u.DoctorRepository.ExistsAsync(appointment.DoctorId)).ReturnsAsync(true);
        _mockUnitOfWork.Setup(u => u.PatientRepository.ExistsAsync(appointment.PatientId)).ReturnsAsync(true);
        _mockUnitOfWork.Setup(u => u.RoomRepository.ExistsAsync(appointment.RoomId!.Value)).ReturnsAsync(false);

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

        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsByDoctorIdAsync(It.IsAny<int>())).ReturnsAsync([]);
        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsByPatientIdAsync(It.IsAny<int>())).ReturnsAsync(existingAppointments);
        _mockUnitOfWork.Setup(u => u.DoctorRepository.ExistsAsync(appointment.DoctorId)).ReturnsAsync(true);
        _mockUnitOfWork.Setup(u => u.PatientRepository.ExistsAsync(appointment.PatientId)).ReturnsAsync(true);
        _mockUnitOfWork.Setup(u => u.RoomRepository.ExistsAsync(appointment.RoomId!.Value)).ReturnsAsync(true);

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

        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsByDoctorIdAsync(It.IsAny<int>())).ReturnsAsync(existingAppointments);
        _mockUnitOfWork.Setup(u => u.DoctorRepository.ExistsAsync(appointment.DoctorId)).ReturnsAsync(true);
        _mockUnitOfWork.Setup(u => u.PatientRepository.ExistsAsync(appointment.PatientId)).ReturnsAsync(true);
        _mockUnitOfWork.Setup(u => u.RoomRepository.ExistsAsync(appointment.RoomId!.Value)).ReturnsAsync(true);

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

        var mockRoomRepository = new Mock<IRoomRepository>();
        _mockUnitOfWork.Setup(uow => uow.RoomRepository).Returns(mockRoomRepository.Object);


        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsByDoctorIdAsync(It.IsAny<int>())).ReturnsAsync([]);
        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsByPatientIdAsync(It.IsAny<int>())).ReturnsAsync([]);
        mockRoomRepository.Setup(r => r.GetRoomAppointmentsAsync(It.IsAny<int>())).ReturnsAsync(existingAppointments.ToList());

        _mockUnitOfWork.Setup(u => u.DoctorRepository.ExistsAsync(appointment.DoctorId)).ReturnsAsync(true);
        _mockUnitOfWork.Setup(u => u.PatientRepository.ExistsAsync(appointment.PatientId)).ReturnsAsync(true);
        _mockUnitOfWork.Setup(u => u.RoomRepository.ExistsAsync(appointment.RoomId!.Value)).ReturnsAsync(true);

        //Act
        var result = await _sut.AddAppointmentAsync(appointment);

        //Assert
        result.Success.ShouldBeFalse();
    }

    #endregion

}
