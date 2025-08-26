using System;

namespace BeHealthy.Tests.UnitTests.Services;

public class AppointmentServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IAppointmentRepository> _mockAppointmentRepository;
    private readonly AppointmentService _sut;

    public AppointmentServiceTests()
    {
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
        IEnumerable<Appointment> appointments = [
            CreateAppointment(),
            CreateAppointment(id: 2)
        ];

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
        IEnumerable<Appointment> appointments = [
            CreateAppointment(),
            CreateAppointment(id: 2)
        ];

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
        IEnumerable<Appointment> appointments = [ 
            CreateAppointment(),
            CreateAppointment(id: 2)
        ];

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
        int patientId = 1) => new Appointment
        {
            Id = id,
            DoctorId = doctorId,
            PatientId = patientId,
            AppointmentDate = DateOnly.FromDateTime(DateTime.Today),
            AppointmentStartTime = TimeOnly.FromDateTime(DateTime.Today),
            AppointmentEndTime = TimeOnly.FromDateTime(DateTime.Today.AddHours(1)),
            Status = AppointmentStatus.Scheduled,
            Reason = AppointmentReason.GeneralCheckup
        };

    #endregion
}
