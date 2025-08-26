using AutoFixture;

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
        IEnumerable<Appointment> appointments = CreateAppointments(count: 2);

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
    public async Task GetAllAppointmentsAsync_ReturnsAppointments()
    {
        //Arrange
        IEnumerable<Appointment> appointments = CreateAppointments(count: 2);

        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsAsync())
            .ReturnsAsync(appointments);

        //Act
        var result = await _sut.GetAllAppointmentsAsync();

        //Assert
        result.ShouldNotBeNull();
    }

    #endregion

    #region GetAllAppointmentsByPatientIdAsync

    [Fact]
    public async Task GetAllAppointmentsByPatientIdAsync_WithValidPatientId_ReturnsAppointments()
    {
        //Arrange
        IEnumerable<Appointment> appointments =  CreateAppointments(count: 2);

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
        var appointment = CreateAppointment();

        _mockAppointmentRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(appointment);

        //Act
        var result = await _sut.GetAppointmentByIdAsync(1);

        //Assert
        result.ShouldNotBeNull();
    }

    #endregion

    #region Helpers

    private Appointment CreateAppointment(
        int id = 1,
        int doctorId = 1,
        int patientId = 1)
    {
        return _fixture
            .Build<Appointment>()
            .Without(a => a.Nurse)
            .Without(a => a.Room)
            .Without(a => a.Doctor)
            .Without(a => a.Patient)
            .With(a => a.AppointmentStartTime, TimeOnly.FromDateTime(DateTime.UtcNow))
            .With(a => a.AppointmentEndTime, TimeOnly.FromDateTime(DateTime.UtcNow.AddHours(1)))
            .Create();
    }

    private IEnumerable<Appointment> CreateAppointments(
        int id = 1,
        int doctorId = 1,
        int patientId = 1,
        int count = 1)
    {
        return _fixture
            .Build<Appointment>()
            .Without(a => a.Nurse)
            .Without(a => a.Room)
            .Without(a => a.Doctor)
            .Without(a => a.Patient)
            .With(a => a.AppointmentStartTime, TimeOnly.FromDateTime(DateTime.UtcNow))
            .With(a => a.AppointmentEndTime, TimeOnly.FromDateTime(DateTime.UtcNow.AddHours(1)))
            .CreateMany(count);
    }

    #endregion
}
