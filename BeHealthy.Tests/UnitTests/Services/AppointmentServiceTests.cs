using BeHealthy.Application.Services;

namespace BeHealthy.Tests.UnitTests.Services;

public class AppointmentServiceTests
{
    private Mock<IAppointmentRepository> CreateAppointmentRepoMock() => new Mock<IAppointmentRepository>();

    private AppointmentService CreateService(Mock<IAppointmentRepository> repoMock)
    {
        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(uow => uow.AppointmentRepository).Returns(repoMock.Object);
        return new AppointmentService(uowMock.Object);
    }

    [Fact]
    public async Task GetAllAppointmentsByDoctorIdAsync_WithValidDoctorId_ReturnsAppointments()
    {
        //Arrange
        var repoMock = CreateAppointmentRepoMock();
        repoMock.Setup(r => r.GetAllAppointmentsByDoctorIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new List<Appointment>
            {
                new Appointment 
                { 
                    Id = 1, 
                    DoctorId = 1, 
                    PatientId = 1, 
                    AppointmentDate = DateOnly.FromDateTime(DateTime.Now),
                    AppointmentStartTime = TimeOnly.FromDateTime(DateTime.Now),
                    AppointmentEndTime = TimeOnly.FromDateTime(DateTime.Now.AddHours(1)),
                    Status = AppointmentStatus.Scheduled,
                    Reason = AppointmentReason.GeneralCheckup}
            });

        var service = CreateService(repoMock);

        //Act
        var result = await service.GetAllAppointmentsByDoctorIdAsync(1);

        //Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetAllAppointmentsByDoctorIdAsync_WithInvalidDoctorId_ReturnsEmptyList()
    {
        //Arrange
        var repoMock = CreateAppointmentRepoMock();
        repoMock.Setup(r => r.GetAllAppointmentsByDoctorIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new List<Appointment>());

        var service = CreateService(repoMock);

        //Act
        var result = await service.GetAllAppointmentsByDoctorIdAsync(-1);

        //Assert
        Assert.Empty(result);
    }
}
