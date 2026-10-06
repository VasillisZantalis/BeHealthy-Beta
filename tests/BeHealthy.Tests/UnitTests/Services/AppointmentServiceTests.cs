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

    [Fact]
    public async Task GetAllAppointmentsAsync_WithDoctorFilter_CountsOnlyMatchingAppointments()
    {
        //Arrange
        Expression<Func<Appointment, bool>>? countPredicate = null;
        _mockAppointmentRepository
            .Setup(r => r.GetCountAsync(It.IsAny<Expression<Func<Appointment, bool>>>(), It.IsAny<CancellationToken>()))
            .Callback<Expression<Func<Appointment, bool>>, CancellationToken>((p, _) => countPredicate = p)
            .ReturnsAsync(2);

        var appointments = new AppointmentBuilder(_fixture).WithDoctorId(1).BuildMany(2)
            .Concat(new AppointmentBuilder(_fixture).WithDoctorId(2).BuildMany(3));

        //Act
        var result = await _sut.GetAllAppointmentsAsync(new Shared.Parameters.AppointmentQueryParameters { DoctorId = 1 });

        //Assert
        result.TotalCount.ShouldBe(2);
        _mockAppointmentRepository.Verify(r => r.GetCountAsync(It.IsAny<CancellationToken>()), Times.Never);
        countPredicate.ShouldNotBeNull();
        appointments.Count(countPredicate.Compile()).ShouldBe(2);
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

    /// <summary>Makes QueryAsync filter these appointments with the service's own predicate, as the database would.</summary>
    private void UseExistingAppointments(params Appointment[] existing) =>
        _mockAppointmentRepository
            .Setup(r => r.QueryAsync(It.IsAny<QueryOptions<Appointment>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((QueryOptions<Appointment> options, CancellationToken _) =>
                (IEnumerable<Appointment>)existing.Where(options.Predicate!.Compile()).ToList());

    private void AllReferencesExist()
    {
        _mockDoctorRepository.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _mockPatientRepository.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _mockRoomRepository.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
    }

    private static readonly DateOnly Day = new(2030, 1, 15);

    private AppointmentBuilder Existing(int startHour, int endHour) => new AppointmentBuilder(_fixture)
        .WithId(100)
        .WithDoctorId(2)
        .WithPatientId(2)
        .WithDate(Day)
        .WithStartTime(new TimeOnly(startHour, 0))
        .WithEndTime(new TimeOnly(endHour, 0));

    private AppointmentCreateDtoBuilder NewAppointment(int startHour, int startMinute, int endHour, int endMinute) => new AppointmentCreateDtoBuilder(_fixture)
        .WithDoctorId(1)
        .WithPatientId(1)
        .WithRoomId(null)
        .WithDate(Day)
        .WithStartTime(new TimeOnly(startHour, startMinute))
        .WithEndTime(new TimeOnly(endHour, endMinute));

    [Fact]
    public async Task AddAppointment_WithDoctorConflict_ReturnsFailedResponse()
    {
        //Arrange
        AllReferencesExist();
        UseExistingAppointments(Existing(10, 11).WithDoctorId(1).Build());

        //Act
        var result = await _sut.AddAppointmentAsync(NewAppointment(10, 30, 11, 30).Build());

        //Assert
        result.Success.ShouldBeFalse();
        _mockAppointmentRepository.Verify(r => r.AddAsync(It.IsAny<Appointment>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task AddAppointment_WithPatientConflict_ReturnsFailedResponse()
    {
        //Arrange
        AllReferencesExist();
        UseExistingAppointments(Existing(10, 11).WithPatientId(1).Build());

        //Act
        var result = await _sut.AddAppointmentAsync(NewAppointment(9, 30, 10, 30).Build());

        //Assert
        result.Success.ShouldBeFalse();
        _mockAppointmentRepository.Verify(r => r.AddAsync(It.IsAny<Appointment>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task AddAppointment_WithNurseConflict_ReturnsFailedResponse()
    {
        //Arrange
        AllReferencesExist();
        UseExistingAppointments(Existing(10, 11).WithNurseId(5).Build());

        //Act
        var result = await _sut.AddAppointmentAsync(NewAppointment(10, 0, 11, 0).WithNurseId(5).Build());

        //Assert
        result.Success.ShouldBeFalse();
    }

    [Fact]
    public async Task AddAppointment_WithRoomConflicts_ReturnsFailedResponse()
    {
        //Arrange
        AllReferencesExist();
        UseExistingAppointments(Existing(10, 11).WithRoomId(1).Build());

        //Act
        var result = await _sut.AddAppointmentAsync(NewAppointment(10, 15, 10, 45).WithRoomId(1).Build());

        //Assert
        result.Success.ShouldBeFalse();
        result.ErrorMessage.ShouldBe(Resource.RoomIsBookedAtThatTime);
    }

    [Fact]
    public async Task AddAppointment_BackToBackWithExisting_Succeeds()
    {
        //Arrange
        AllReferencesExist();
        UseExistingAppointments(Existing(10, 11).WithDoctorId(1).WithPatientId(1).Build());

        //Act
        var result = await _sut.AddAppointmentAsync(NewAppointment(11, 0, 12, 0).Build());

        //Assert
        result.Success.ShouldBeTrue();
    }

    [Fact]
    public async Task AddAppointment_OverlapsCancelledAppointment_Succeeds()
    {
        //Arrange
        AllReferencesExist();
        UseExistingAppointments(Existing(10, 11).WithDoctorId(1).WithStatus(AppointmentStatus.Cancelled).Build());

        //Act
        var result = await _sut.AddAppointmentAsync(NewAppointment(10, 0, 11, 0).Build());

        //Assert
        result.Success.ShouldBeTrue();
    }

    [Fact]
    public async Task AddAppointment_SameTimeOnAnotherDay_Succeeds()
    {
        //Arrange
        AllReferencesExist();
        UseExistingAppointments(Existing(10, 11).WithDoctorId(1).WithDate(Day.AddDays(1)).Build());

        //Act
        var result = await _sut.AddAppointmentAsync(NewAppointment(10, 0, 11, 0).Build());

        //Assert
        result.Success.ShouldBeTrue();
    }

    [Fact]
    public async Task AddAppointment_NewAppointmentIsCancelled_SkipsConflictCheck()
    {
        //Arrange
        AllReferencesExist();
        UseExistingAppointments(Existing(10, 11).WithDoctorId(1).Build());

        //Act
        var result = await _sut.AddAppointmentAsync(NewAppointment(10, 0, 11, 0).WithStatus(AppointmentStatus.Cancelled).Build());

        //Assert
        result.Success.ShouldBeTrue();
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
    public async Task UpdateAppointmentAsync_WithConflictingOtherAppointment_ReturnsFailedResponse()
    {
        //Arrange
        AllReferencesExist();
        _mockAppointmentRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Appointment { Id = 1 });
        UseExistingAppointments(Existing(10, 11).WithDoctorId(1).Build());

        var update = new AppointmentUpdateDtoBuilder(_fixture)
            .WithId(1)
            .WithDoctorId(1)
            .WithRoomId(null)
            .WithDate(Day)
            .WithStartTime(new TimeOnly(10, 30))
            .WithEndTime(new TimeOnly(11, 30))
            .Build();

        //Act
        var result = await _sut.UpdateAppointmentAsync(update);

        //Assert
        result.Success.ShouldBeFalse();
        _mockAppointmentRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAppointmentAsync_OverlapsOnlyItself_Succeeds()
    {
        //Arrange
        AllReferencesExist();
        _mockAppointmentRepository.Setup(r => r.GetByIdAsync(100, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Appointment { Id = 100 });
        UseExistingAppointments(Existing(10, 11).WithDoctorId(1).WithPatientId(1).Build());

        var update = new AppointmentUpdateDtoBuilder(_fixture)
            .WithId(100)
            .WithDoctorId(1)
            .WithPatientId(1)
            .WithRoomId(null)
            .WithDate(Day)
            .WithStartTime(new TimeOnly(10, 30))
            .WithEndTime(new TimeOnly(11, 30))
            .Build();

        //Act
        var result = await _sut.UpdateAppointmentAsync(update);

        //Assert
        result.Success.ShouldBeTrue();
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
        var result = await _sut.DeleteAppointmentAsync(appointmentId);

        //Assert
        result.Success.ShouldBeTrue();
        _mockAppointmentRepository.Verify(r => r.DeleteAsync(appointmentId, It.IsAny<CancellationToken>()), Times.Once);
        _mockAppointmentRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAppointmentAsync_NotFound_ReturnsFailedAndDoesNotSave()
    {
        //Arrange
        _mockAppointmentRepository.Setup(r => r.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        //Act
        var result = await _sut.DeleteAppointmentAsync(1);

        //Assert
        result.Success.ShouldBeFalse();
        result.ErrorMessage.ShouldBe(string.Format(Resource.NotFoundEntity, Resource.Appointment));
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

    #region GetUpcomingAppointmentsAsync

    [Fact]
    public async Task GetUpcomingAppointmentsAsync_IncludesDoctorAndPatientAndLimitsToFive()
    {
        //Arrange
        QueryOptions<Appointment>? captured = null;
        _mockAppointmentRepository
            .Setup(r => r.QueryAsync(It.IsAny<QueryOptions<Appointment>>(), It.IsAny<CancellationToken>()))
            .Callback<QueryOptions<Appointment>, CancellationToken>((o, _) => captured = o)
            .ReturnsAsync(new List<Appointment>());

        //Act
        await _sut.GetUpcomingAppointmentsAsync();

        //Assert
        captured.ShouldNotBeNull();
        captured.Includes.Count.ShouldBe(2);
        captured.PageNumber.ShouldBe(1);
        captured.PageSize.ShouldBe(5);
    }

    #endregion

    #region GetAppointmentReasonCounts

    #endregion
}
