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
        _fixture.Customize<Appointment>(c =>
            c.With(a => a.AppointmentDate, DateOnly.FromDateTime(DateTime.Today))
            );

        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockAppointmentRepository = new Mock<IAppointmentRepository>();

        _mockUnitOfWork.Setup(uow => uow.AppointmentRepository).Returns(_mockAppointmentRepository.Object);

        _sut = new AppointmentService(_mockUnitOfWork.Object);
    }

    #region GetAllAppointmentsByDoctorIdAsync

    [Fact]
    public async Task GetAllAppointmentsByDoctorIdAsync_WithValidDoctorId_ReturnsAppointments()
    {
        var appointments = CreateAppointments(3);
        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsByDoctorIdAsync(It.IsAny<int>()))
            .ReturnsAsync(appointments);

        var result = await _sut.GetAllAppointmentsByDoctorIdAsync(1);

        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetAllAppointmentsByDoctorIdAsync_WithInvalidDoctorId_ReturnsEmptyList()
    {
        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsByDoctorIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new List<Appointment>());

        var result = await _sut.GetAllAppointmentsByDoctorIdAsync(-1);

        result.ShouldBeEmpty();
    }

    #endregion

    #region GetAllAppointmentsAsync

    [Fact]
    public async Task GetAllAppointmentsAsync_ReturnsAppointments()
    {
        var appointments = _fixture.CreateMany<Appointment>(2).ToList();
        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsAsync())
            .ReturnsAsync(appointments);

        var result = await _sut.GetAllAppointmentsAsync();

        result.ShouldNotBeNull();
    }

    #endregion

    #region GetAllAppointmentsByPatientIdAsync

    [Fact]
    public async Task GetAllAppointmentsByPatientIdAsync_WithValidPatientId_ReturnsAppointments()
    {
        var appointments = _fixture.CreateMany<Appointment>(2).ToList();
        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsByPatientIdAsync(It.IsAny<int>()))
            .ReturnsAsync(appointments);

        var result = await _sut.GetAllAppointmentsByPatientIdAsync(1);

        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetAllAppointmentsByPatientIdAsync_WithInvalidPatientId_ReturnsEmptyList()
    {
        _mockAppointmentRepository.Setup(r => r.GetAllAppointmentsByPatientIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new List<Appointment>());

        var result = await _sut.GetAllAppointmentsByPatientIdAsync(-1);

        result.ShouldBeEmpty();
    }

    #endregion

    #region GetAppointmentByIdAsync

    [Fact]
    public async Task GetAppointmentByIdAsync_WithValidId_ReturnsAppointment()
    {
        var appointment = _fixture.Create<Appointment>();
        _mockAppointmentRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(appointment);

        var result = await _sut.GetAppointmentByIdAsync(1);

        result.ShouldNotBeNull();
    }

    #endregion

    #region Helpers

    private Appointment CreateValidAppointment() => _fixture.Create<Appointment>();

    private List<Appointment> CreateAppointments(int count) =>
        _fixture.CreateMany<Appointment>(count).ToList();

    #endregion
}
