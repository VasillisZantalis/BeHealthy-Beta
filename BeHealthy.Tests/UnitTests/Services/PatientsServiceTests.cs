namespace BeHealthy.Tests.UnitTests.Services;

public class PatientsServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<IPatientRepository> _patientRepositoryMock;
    private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
    private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
    private readonly PatientService _service;

    public PatientsServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _userServiceMock = new Mock<IUserService>();
        _patientRepositoryMock = new Mock<IPatientRepository>();
        _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
        _doctorRepositoryMock = new Mock<IDoctorRepository>();

        _unitOfWorkMock.SetupGet(u => u.PatientRepository).Returns(_patientRepositoryMock.Object);
        _unitOfWorkMock.SetupGet(u => u.AppointmentRepository).Returns(_appointmentRepositoryMock.Object);
        _unitOfWorkMock.SetupGet(u => u.DoctorRepository).Returns(_doctorRepositoryMock.Object);

        _service = new PatientService(_unitOfWorkMock.Object, _userServiceMock.Object);
    }

    #region GetAllPatientsAsync

    [Fact]
    public async Task GetAllPatientsAsync_ReturnsMappedDtos()
    {
        // Arrange
        var patients = new List<Patient> { new Patient { Id = 1 } };
        _patientRepositoryMock.Setup(r => r.GetAllPatientsAsync()).ReturnsAsync(patients);

        // Act
        var result = await _service.GetAllPatientsAsync();

        // Assert
        Assert.NotNull(result);
        _patientRepositoryMock.Verify(r => r.GetAllPatientsAsync(), Times.Once);
    }

    #endregion

    #region GetAllPatientsSimpleAsync

    [Fact]
    public async Task GetAllPatientsSimpleAsync_ReturnsMappedSimpleDtos()
    {
        // Arrange
        var patients = new List<Patient> { new Patient { Id = 1 } };
        _patientRepositoryMock.Setup(r => r.GetAllPatientsSimpleAsync()).ReturnsAsync(patients);

        // Act
        var result = await _service.GetAllPatientsSimpleAsync();

        // Assert
        Assert.NotNull(result);
        _patientRepositoryMock.Verify(r => r.GetAllPatientsSimpleAsync(), Times.Once);
    }

    #endregion

    #region GetPatientByIdAsync

    [Fact]
    public async Task GetPatientByIdAsync_ReturnsMappedDto_WhenPatientExists()
    {
        // Arrange
        var patient = new Patient { Id = 1 };
        _patientRepositoryMock.Setup(r => r.GetByIdWithIncludes(1, It.IsAny<Expression<Func<Patient, object>>[]>()))
            .ReturnsAsync(patient);

        // Act
        var result = await _service.GetPatientByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        _patientRepositoryMock.Verify(r => r.GetByIdWithIncludes(
            1,
            It.IsAny<Expression<Func<Patient, object>>[]>()),
            Times.Once);
    }

    [Fact]
    public async Task GetPatientByIdAsync_ReturnsNull_WhenPatientDoesNotExist()
    {
        // Arrange
        _patientRepositoryMock.Setup(r => r.GetByIdWithIncludes(
            2, 
            It.IsAny<Expression<Func<Patient, object>>[]>())
        )
        .ReturnsAsync((Patient?)null);

        // Act
        var result = await _service.GetPatientByIdAsync(2);

        // Assert
        Assert.Null(result);
    }

    #endregion

    #region AddPatientAsync

    [Fact]
    public async Task AddPatientAsync_ReturnsSuccess_WhenAllStepsSucceed()
    {
        // Arrange
        var patientDto = new PatientCreateRequest { Email = "test@test.com", Password = "pass" };
        var user = new ApplicationUser { Id = "user1" };

        _userServiceMock.Setup(s => s.CreateApplicationUser(
            It.IsAny<ApplicationUser>(),
            patientDto.Password,
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(ServiceResponse.Successful());

        _userServiceMock.Setup(s => s.AddUserToRoleAsync(It.IsAny<ApplicationUser>(), UserRole.Patient))
            .ReturnsAsync(ServiceResponse.Successful());
        
        _patientRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Patient>())).Returns(Task.CompletedTask);

        // Act
        var result = await _service.AddPatientAsync(patientDto);

        // Assert
        Assert.True(result.Success);
        _userServiceMock.Verify(s => s.CreateApplicationUser(
            It.IsAny<ApplicationUser>(),
            patientDto.Password,
            It.IsAny<CancellationToken>()), Times.Once);

        _userServiceMock.Verify(s => s.AddUserToRoleAsync(It.IsAny<ApplicationUser>(), UserRole.Patient), Times.Once);
        _patientRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Patient>()), Times.Once);
    }

    [Fact]
    public async Task AddPatientAsync_ReturnsFailed_WhenUserCreationFails()
    {
        // Arrange
        var patientDto = new PatientCreateRequest { Email = "fail@test.com", Password = "pass" };
        _userServiceMock.Setup(s => s.CreateApplicationUser(
            It.IsAny<ApplicationUser>(),
            patientDto.Password,
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(ServiceResponse.Failed("error"));

        // Act
        var result = await _service.AddPatientAsync(patientDto);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("error", result.ErrorMessage);
    }

    [Fact]
    public async Task AddPatientAsync_ReturnsFailed_WhenAddToRoleFails()
    {
        // Arrange
        var patientDto = new PatientCreateRequest { Email = "failrole@test.com", Password = "pass" };
        _userServiceMock.Setup(s => s.CreateApplicationUser(
            It.IsAny<ApplicationUser>(),
            patientDto.Password,
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(ServiceResponse.Successful());
        
        _userServiceMock.Setup(s => s.AddUserToRoleAsync(It.IsAny<ApplicationUser>(), UserRole.Patient))
            .ReturnsAsync(ServiceResponse.Failed("role error"));

        // Act
        var result = await _service.AddPatientAsync(patientDto);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("role error", result.ErrorMessage);
    }

    [Fact]
    public async Task AddPatientAsync_DeletesUserOnException()
    {
        // Arrange
        var patientDto = new PatientCreateRequest { Email = "exception@test.com", Password = "pass" };
        _userServiceMock.Setup(s => s.CreateApplicationUser(
            It.IsAny<ApplicationUser>(),
            patientDto.Password,
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(ServiceResponse.Successful());

        _userServiceMock.Setup(s => s.AddUserToRoleAsync(It.IsAny<ApplicationUser>(), UserRole.Patient))
            .ReturnsAsync(ServiceResponse.Successful());

        _patientRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Patient>()))
            .ThrowsAsync(new Exception());

        _userServiceMock.Setup(s => s.DeleteUserAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(ServiceResponse.Successful());

        // Act
        var result = await _service.AddPatientAsync(patientDto);

        // Assert
        Assert.False(result.Success);
        _userServiceMock.Verify(s => s.DeleteUserAsync(It.IsAny<ApplicationUser>()), Times.Once);
    }

    #endregion

    #region UpdatePatientAsync

    [Fact]
    public async Task UpdatePatientAsync_ReturnsSuccess_WhenUpdateSucceeds()
    {
        // Arrange
        var patientDto = new PatientUpdateRequest { UserId = "user1", FirstName = "John", LastName = "Doe", PhoneNumber = "123" };
        var user = new ApplicationUser { Id = "user1" };

        _userServiceMock.Setup(s => s.GetUserByIdAsync(patientDto.UserId))
            .ReturnsAsync(user);

        _userServiceMock.Setup(s => s.UpdateUserAsync(user, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ServiceResponse.Successful());

        _patientRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Patient>())).Returns(Task.CompletedTask);

        // Act
        var result = await _service.UpdatePatientAsync(patientDto);

        // Assert
        Assert.True(result.Success);
        _userServiceMock.Verify(s => s.GetUserByIdAsync(patientDto.UserId), Times.Once);
        _userServiceMock.Verify(s => s.UpdateUserAsync(user, It.IsAny<CancellationToken>()), Times.Once);
        _patientRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Patient>()), Times.Once);
    }

    [Fact]
    public async Task UpdatePatientAsync_ReturnsFailed_WhenUserNotFound()
    {
        // Arrange
        var patientDto = new PatientUpdateRequest { UserId = "notfound" };
        
        _userServiceMock.Setup(s => s.GetUserByIdAsync(patientDto.UserId))
            .ReturnsAsync((ApplicationUser?)null);

        // Act
        var result = await _service.UpdatePatientAsync(patientDto);

        // Assert
        Assert.False(result.Success);
    }

    [Fact]
    public async Task UpdatePatientAsync_ReturnsFailed_WhenUpdateUserFails()
    {
        // Arrange
        var patientDto = new PatientUpdateRequest { UserId = "user1" };
        var user = new ApplicationUser { Id = "user1" };

        _userServiceMock.Setup(s => s.GetUserByIdAsync(patientDto.UserId))
            .ReturnsAsync(user);
        
        _userServiceMock.Setup(s => s.UpdateUserAsync(user, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ServiceResponse.Failed("update error"));

        // Act
        var result = await _service.UpdatePatientAsync(patientDto);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("update error", result.ErrorMessage);
    }

    #endregion

    #region DeletePatientAsync

    [Fact]
    public async Task DeletePatientAsync_CallsRepository()
    {
        // Arrange
        _patientRepositoryMock.Setup(r => r.DeletePatientAsync(1)).Returns(Task.CompletedTask);

        // Act
        await _service.DeletePatientAsync(1);

        // Assert
        _patientRepositoryMock.Verify(r => r.DeletePatientAsync(1), Times.Once);
    }

    #endregion

    #region GetPatientAppointmentsByUserIdAsync

    [Fact]
    public async Task GetPatientAppointmentsByUserIdAsync_ReturnsMappedDtos()
    {
        // Arrange
        var appointments = new List<Appointment> { new Appointment { Id = 1 } };
        _patientRepositoryMock.Setup(r => r.GetPatientAppointmentsByUserIdAsync("user1")).ReturnsAsync(appointments);

        // Act
        var result = await _service.GetPatientAppointmentsByUserIdAsync("user1");

        // Assert
        Assert.NotNull(result);
        _patientRepositoryMock.Verify(r => r.GetPatientAppointmentsByUserIdAsync("user1"), Times.Once);
    }

    #endregion

    #region GetMyDoctorsAsync

    [Fact]
    public async Task GetMyDoctorsAsync_ReturnsEmpty_WhenPatientNotFound()
    {
        // Arrange
        _patientRepositoryMock.Setup(r => r.GetByUserIdAsync("user1"))
            .ReturnsAsync((Patient?)null);

        // Act
        var result = await _service.GetMyDoctorsAsync("user1");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetMyDoctorsAsync_ReturnsDoctors_WhenPatientAndAppointmentsExist()
    {
        // Arrange
        var patient = new Patient { Id = 1 };
        var appointments = new List<Appointment>
        {
            new Appointment { DoctorId = 2 },
            new Appointment { DoctorId = 3 }
        };
        var doctors = new List<Doctor>
        {
            new Doctor { Id = 2 },
            new Doctor { Id = 3 }
        };

        _patientRepositoryMock.Setup(r => r.GetByUserIdAsync("user1"))
            .ReturnsAsync(patient);

        _appointmentRepositoryMock.Setup(r => r.GetAllAppointmentsByPatientIdAsync(patient.Id))
            .ReturnsAsync(appointments);

        _doctorRepositoryMock.Setup(r => r.QueryAsync(It.IsAny<QueryOptions<Doctor>>())).ReturnsAsync(doctors);

        // Act
        var result = await _service.GetMyDoctorsAsync("user1");

        // Assert
        Assert.NotNull(result);
        _doctorRepositoryMock.Verify(r => r.QueryAsync(It.IsAny<QueryOptions<Doctor>>()), Times.Once);
    }

    #endregion

    #region GetPatientCountAsync

    [Fact]
    public async Task GetPatientCountAsync_ReturnsCount()
    {
        // Arrange
        _patientRepositoryMock.Setup(r => r.GetCountAsync()).ReturnsAsync(5);

        // Act
        var result = await _service.GetPatientCountAsync();

        // Assert
        Assert.Equal(5, result);
        _patientRepositoryMock.Verify(r => r.GetCountAsync(), Times.Once);
    }

    #endregion
}
