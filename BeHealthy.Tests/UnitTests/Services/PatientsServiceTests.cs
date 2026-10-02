using BeHealthy.Tests.UnitTests.Fakes;

namespace BeHealthy.Tests.UnitTests.Services;

public class PatientsServiceTests
{
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<IPatientRepository> _patientRepositoryMock;
    private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
    private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
    private readonly FakeTransactionManager _transactionManager;
    private readonly PatientService _service;

    public PatientsServiceTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _patientRepositoryMock = new Mock<IPatientRepository>();
        _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
        _doctorRepositoryMock = new Mock<IDoctorRepository>();
        _transactionManager = new FakeTransactionManager();

        _service = new PatientService(
            _patientRepositoryMock.Object,
            _appointmentRepositoryMock.Object,
            _doctorRepositoryMock.Object,
            _userServiceMock.Object,
            _transactionManager);
    }

    #region GetAllPatientsAsync

    [Fact]
    public async Task GetAllPatientsAsync_ReturnsMappedDtos()
    {
        // Arrange
        var patients = new List<Patient> { new Patient { Id = 1 } };
        _patientRepositoryMock.Setup(r => r.QueryAsync(It.IsAny<QueryOptions<Patient>>(), It.IsAny<CancellationToken>())).ReturnsAsync(patients);

        // Act
        var result = await _service.GetAllPatientsAsync();

        // Assert
        Assert.NotNull(result);
        _patientRepositoryMock.Verify(r => r.QueryAsync(It.IsAny<QueryOptions<Patient>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region GetAllPatientsSimpleAsync

    [Fact]
    public async Task GetAllPatientsSimpleAsync_ReturnsMappedSimpleDtos()
    {
        // Arrange
        var patients = new List<Patient> { new Patient { Id = 1 } };
        _patientRepositoryMock.Setup(r => r.GetAllPatientsSimpleAsync(It.IsAny<CancellationToken>())).ReturnsAsync(patients);

        // Act
        var result = await _service.GetAllPatientsSimpleAsync();

        // Assert
        Assert.NotNull(result);
        _patientRepositoryMock.Verify(r => r.GetAllPatientsSimpleAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region GetPatientByIdAsync

    [Fact]
    public async Task GetPatientByIdAsync_ReturnsMappedDto_WhenPatientExists()
    {
        // Arrange
        var patient = new Patient { Id = 1 };
        _patientRepositoryMock.Setup(r => r.GetByIdWithIncludes(1, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Patient, object>>[]>()))
            .ReturnsAsync(patient);

        // Act
        var result = await _service.GetPatientByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        _patientRepositoryMock.Verify(r => r.GetByIdWithIncludes(
            1, It.IsAny<CancellationToken>(),
            It.IsAny<Expression<Func<Patient, object>>[]>()),
            Times.Once);
    }

    [Fact]
    public async Task GetPatientByIdAsync_ReturnsNull_WhenPatientDoesNotExist()
    {
        // Arrange
        _patientRepositoryMock.Setup(r => r.GetByIdWithIncludes(
            2, It.IsAny<CancellationToken>(),
            It.IsAny<Expression<Func<Patient, object>>[]>())
        )
        .ReturnsAsync((Patient?)null);

        // Act
        var result = await _service.GetPatientByIdAsync(2);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetPatientByIdAsync_ForwardsCancellationToken()
    {
        // Arrange
        using var cts = new CancellationTokenSource();

        // Act
        await _service.GetPatientByIdAsync(1, cts.Token);

        // Assert
        _patientRepositoryMock.Verify(r => r.GetByIdWithIncludes(
            1,
            cts.Token,
            It.IsAny<Expression<Func<Patient, object>>[]>()),
            Times.Once);
    }

    #endregion

    #region AddPatientAsync

    [Fact]
    public async Task AddPatientAsync_ForwardsCancellationTokenToEveryStep()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var patientDto = new PatientCreateRequest { Email = "token@test.com", Password = "pass" };

        _userServiceMock.Setup(s => s.CreateApplicationUser(It.IsAny<ApplicationUser>(), patientDto.Password, cts.Token))
            .ReturnsAsync(ServiceResponse.Successful());
        _userServiceMock.Setup(s => s.AddUserToRoleAsync(It.IsAny<ApplicationUser>(), UserRole.Patient, cts.Token))
            .ReturnsAsync(ServiceResponse.Successful());

        // Act
        var result = await _service.AddPatientAsync(patientDto, cts.Token);

        // Assert
        Assert.True(result.Success);
        _patientRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Patient>(), cts.Token), Times.Once);
        _patientRepositoryMock.Verify(r => r.SaveChangesAsync(cts.Token), Times.Once);
    }

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

        _userServiceMock.Setup(s => s.AddUserToRoleAsync(It.IsAny<ApplicationUser>(), UserRole.Patient, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ServiceResponse.Successful());

        _patientRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Patient>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // Act
        var result = await _service.AddPatientAsync(patientDto);

        // Assert
        Assert.True(result.Success);
        _userServiceMock.Verify(s => s.CreateApplicationUser(
            It.IsAny<ApplicationUser>(),
            patientDto.Password,
            It.IsAny<CancellationToken>()), Times.Once);

        _userServiceMock.Verify(s => s.AddUserToRoleAsync(It.IsAny<ApplicationUser>(), UserRole.Patient, It.IsAny<CancellationToken>()), Times.Once);
        _patientRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Patient>(), It.IsAny<CancellationToken>()), Times.Once);
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

        _userServiceMock.Setup(s => s.AddUserToRoleAsync(It.IsAny<ApplicationUser>(), UserRole.Patient, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ServiceResponse.Failed("role error"));

        // Act
        var result = await _service.AddPatientAsync(patientDto);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("role error", result.ErrorMessage);
    }

    [Fact]
    public async Task AddPatientAsync_RollsBackTransactionOnException()
    {
        // Arrange
        var patientDto = new PatientCreateRequest { Email = "exception@test.com", Password = "pass" };
        _userServiceMock.Setup(s => s.CreateApplicationUser(
            It.IsAny<ApplicationUser>(),
            patientDto.Password,
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(ServiceResponse.Successful());

        _userServiceMock.Setup(s => s.AddUserToRoleAsync(It.IsAny<ApplicationUser>(), UserRole.Patient, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ServiceResponse.Successful());

        _patientRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Patient>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.AddPatientAsync(patientDto));

        Assert.True(_transactionManager.RolledBack);
        Assert.False(_transactionManager.Committed);
        _patientRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    #endregion

    #region UpdatePatientAsync

    [Fact]
    public async Task UpdatePatientAsync_ReturnsSuccess_WhenUpdateSucceeds()
    {
        // Arrange
        var patientDto = new PatientUpdateRequest { UserId = "user1", FirstName = "John", LastName = "Doe", PhoneNumber = "123" };
        var user = new ApplicationUser { Id = "user1" };

        _userServiceMock.Setup(s => s.GetUserByIdAsync(patientDto.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _userServiceMock.Setup(s => s.UpdateUserAsync(user, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ServiceResponse.Successful());

        _patientRepositoryMock.Setup(r => r.GetByIdAsync(patientDto.Id, It.IsAny<CancellationToken>())).ReturnsAsync(new Patient { Id = patientDto.Id });
        _patientRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Patient>())).Returns(Task.CompletedTask);

        // Act
        var result = await _service.UpdatePatientAsync(patientDto);

        // Assert
        Assert.True(result.Success);
        Assert.True(_transactionManager.Committed);
        _userServiceMock.Verify(s => s.GetUserByIdAsync(patientDto.UserId, It.IsAny<CancellationToken>()), Times.Once);
        _userServiceMock.Verify(s => s.UpdateUserAsync(user, It.IsAny<CancellationToken>()), Times.Once);
        _patientRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdatePatientAsync_ReturnsFailed_WhenUserNotFound()
    {
        // Arrange
        var patientDto = new PatientUpdateRequest { UserId = "notfound" };

        _userServiceMock.Setup(s => s.GetUserByIdAsync(patientDto.UserId, It.IsAny<CancellationToken>()))
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

        _userServiceMock.Setup(s => s.GetUserByIdAsync(patientDto.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _userServiceMock.Setup(s => s.UpdateUserAsync(user, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ServiceResponse.Failed("update error"));

        _patientRepositoryMock.Setup(r => r.GetByIdAsync(patientDto.Id, It.IsAny<CancellationToken>())).ReturnsAsync(new Patient { Id = patientDto.Id });

        // Act
        var result = await _service.UpdatePatientAsync(patientDto);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("update error", result.ErrorMessage);
        Assert.True(_transactionManager.RolledBack);
        _patientRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    #endregion

    #region DeletePatientAsync

    [Fact]
    public async Task DeletePatientAsync_CallsRepository()
    {
        // Arrange
        _patientRepositoryMock.Setup(r => r.DeletePatientAsync(1, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // Act
        await _service.DeletePatientAsync(1);

        // Assert
        _patientRepositoryMock.Verify(r => r.DeletePatientAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region GetPatientAppointmentsByUserIdAsync

    [Fact]
    public async Task GetPatientAppointmentsByUserIdAsync_ReturnsMappedDtos()
    {
        // Arrange
        var appointments = new List<Appointment> { new Appointment { Id = 1 } };
        _patientRepositoryMock.Setup(r => r.GetPatientAppointmentsByUserIdAsync("user1", It.IsAny<CancellationToken>())).ReturnsAsync(appointments);

        // Act
        var result = await _service.GetPatientAppointmentsByUserIdAsync("user1");

        // Assert
        Assert.NotNull(result);
        _patientRepositoryMock.Verify(r => r.GetPatientAppointmentsByUserIdAsync("user1", It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region GetMyDoctorsAsync

    [Fact]
    public async Task GetMyDoctorsAsync_ReturnsEmpty_WhenPatientNotFound()
    {
        // Arrange
        _patientRepositoryMock.Setup(r => r.GetByUserIdAsync("user1", It.IsAny<CancellationToken>()))
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

        _patientRepositoryMock.Setup(r => r.GetByUserIdAsync("user1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);

        _appointmentRepositoryMock.Setup(r => r.GetAllAppointmentsByPatientIdAsync(patient.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(appointments);

        _doctorRepositoryMock.Setup(r => r.QueryAsync(It.IsAny<QueryOptions<Doctor>>(), It.IsAny<CancellationToken>())).ReturnsAsync(doctors);

        // Act
        var result = await _service.GetMyDoctorsAsync("user1");

        // Assert
        Assert.NotNull(result);
        _doctorRepositoryMock.Verify(r => r.QueryAsync(It.IsAny<QueryOptions<Doctor>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region GetPatientCountAsync

    [Fact]
    public async Task GetPatientCountAsync_ReturnsCount()
    {
        // Arrange
        _patientRepositoryMock.Setup(r => r.GetCountAsync(It.IsAny<CancellationToken>())).ReturnsAsync(5);

        // Act
        var result = await _service.GetPatientCountAsync();

        // Assert
        Assert.Equal(5, result);
        _patientRepositoryMock.Verify(r => r.GetCountAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion
}
