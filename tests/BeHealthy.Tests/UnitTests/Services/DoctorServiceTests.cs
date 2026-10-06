using BeHealthy.Shared.Dtos.Appointment;
using BeHealthy.Shared.Dtos.User;
using System.Threading;
using BeHealthy.Tests.UnitTests.Fakes;

namespace BeHealthy.Tests.UnitTests.Services;

public class DoctorServiceTests
{
    private readonly Mock<IDoctorRepository> _mockDoctorRepository;
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<ISpecialtyRepository> _mockSpecialtyRepository;
    private readonly FakeTransactionManager _transactionManager;

    private readonly DoctorService _sut;

    public DoctorServiceTests()
    {
        _mockDoctorRepository = new Mock<IDoctorRepository>();
        _mockUserService = new Mock<IUserService>();
        _mockSpecialtyRepository = new Mock<ISpecialtyRepository>();
        _transactionManager = new FakeTransactionManager();

        _sut = new DoctorService(
            _mockDoctorRepository.Object,
            _mockSpecialtyRepository.Object,
            Mock.Of<IAppointmentRepository>(),
            Mock.Of<IPatientRepository>(),
            _mockUserService.Object,
            _transactionManager);
    }

    #region GetAllDoctorsAsync

    [Fact]
    public async Task GetAllDoctorsAsync_ListFilled_ReturnsListDoctorResponse()
    {
        // Arrange
        var doctors = new List<Doctor>
        {
            new Doctor
            {
                Id = 1,
                UserId = "1",
                FirstName = "John",
                LastName = "Doe",
                SpecialtyId = 1,
                DepartmentId = 1,
                CreatedAt = DateTime.Now,
                User = new ApplicationUser { PhoneNumber = "1234567890", Email = "john.doe@example.com" },
                Specialty = new Specialty { Name = "Cardiology" }
            },
            new Doctor
            {
                Id = 2,
                UserId = "2",
                FirstName = "John",
                LastName = "Does",
                SpecialtyId = 1,
                DepartmentId = 1,
                CreatedAt = DateTime.Now,
                User = new ApplicationUser { PhoneNumber = "1234567890", Email = "john.doe@example.com" },
                Specialty = new Specialty { Name = "Cardiology" }
            }
        };

        _mockDoctorRepository
            .Setup(repo => repo.QueryAsync(It.IsAny<QueryOptions<Doctor>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(doctors);

        // Act
        var result = await _sut.GetAllDoctorsAsync();

        // Assert
        result.ShouldNotBeNull();
        result.Items.ShouldBeAssignableTo<IEnumerable<DoctorResponse>>();
        result.Items.Count().ShouldBe(doctors.Count);
    }

    [Fact]
    public async Task GetAllDoctorsAsync_EmptyList_ReturnsEmptyListOfDoctorResponse()
    {
        //Arrange
        _mockDoctorRepository
            .Setup(repo => repo.GetAllDoctorsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Doctor>());

        //Act
        var result = await _sut.GetAllDoctorsAsync();

        //Assert
        result.Items.ShouldBeEmpty();
    }

    [Fact]
    public async Task GetAllDoctorsAsync_WithFilter_CountsOnlyMatchingDoctors()
    {
        // Arrange
        Expression<Func<Doctor, bool>>? countPredicate = null;
        _mockDoctorRepository
            .Setup(r => r.GetCountAsync(It.IsAny<Expression<Func<Doctor, bool>>>(), It.IsAny<CancellationToken>()))
            .Callback<Expression<Func<Doctor, bool>>, CancellationToken>((p, _) => countPredicate = p)
            .ReturnsAsync(1);

        var doctors = new[]
        {
            new Doctor { FirstName = "Anna", LastName = "Smith", SpecialtyId = 1 },
            new Doctor { FirstName = "Anna", LastName = "Jones", SpecialtyId = 2 },
            new Doctor { FirstName = "Bob", LastName = "Smith", SpecialtyId = 1 },
        };

        // Act
        var result = await _sut.GetAllDoctorsAsync(new Shared.Parameters.DoctorQueryParameters { SearchTerm = "Anna", SpecialtyId = 1 });

        // Assert
        result.TotalCount.ShouldBe(1);
        _mockDoctorRepository.Verify(r => r.GetCountAsync(It.IsAny<CancellationToken>()), Times.Never);
        countPredicate.ShouldNotBeNull();
        doctors.Count(countPredicate.Compile()).ShouldBe(1);
    }

    #endregion

    #region GetDoctorByIdAsync

    [Fact]
    public async Task GetDoctorByIdAsync_ValidId_ReturnsMappedDoctor()
    {
        // Arrange
        var doctorId = 1;
        var doctor = new Doctor
        {
            Id = doctorId,
            UserId = "1",
            FirstName = "John",
            LastName = "Doe",
            SpecialtyId = 1,
            DepartmentId = 1,
            CreatedAt = DateTime.Now,
            User = new ApplicationUser { PhoneNumber = "1234567890", Email = "john.doe@example.com" },
            Specialty = new Specialty { Name = "Cardiology" }
        };

        _mockDoctorRepository.Setup(r => r.GetByIdWithIncludes(doctorId, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Doctor, object>>[]>())).ReturnsAsync(doctor);

        // Act
        var result = await _sut.GetDoctorByIdAsync(doctorId);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<DoctorResponse>();
        result.Id.ShouldBe(doctor.Id);
        result.FirstName.ShouldBe(doctor.FirstName);
        result.LastName.ShouldBe(doctor.LastName);
        result.Email.ShouldBe(doctor.User.Email);
    }

    [Fact]
    public async Task GetDoctorByIdAsync_InvalidId_ReturnsNull()
    {
        // Arrange
        _mockDoctorRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync(() => null);

        // Act
        var result = await _sut.GetDoctorByIdAsync(9999);

        // Assert
        result.ShouldBeNull();
    }

    #endregion

    #region AddDoctorAsync

    [Fact]
    public async Task AddDoctorAsync_Successful_ReturnsSuccessfulResponse()
    {
        // Arrange
        var doctorDto = new DoctorCreateRequest
        {
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@example.com",
            Password = "Password123",
            PhoneNumber = "1234567890"
        };

        var user = new ApplicationUser
        {
            Id = "user-1",
            FirstName = doctorDto.FirstName,
            LastName = doctorDto.LastName,
            Email = doctorDto.Email,
            PhoneNumber = doctorDto.PhoneNumber
        };

        _mockUserService.Setup(s => s.CreateApplicationUser(
            It.IsAny<ApplicationUser>(),
            doctorDto.Password,
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(ServiceResponse.Successful());

        _mockUserService.Setup(s => s.AddUserToRoleAsync(It.IsAny<ApplicationUser>(), UserRole.Doctor, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ServiceResponse.Successful());

        _mockDoctorRepository.Setup(r => r.AddAsync(It.IsAny<Doctor>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _sut.AddDoctorAsync(doctorDto);

        // Assert
        result.ShouldNotBeNull();
        result.Success.ShouldBeTrue();
    }

    [Fact]
    public async Task AddDoctorAsync_UserCreationFails_ReturnsFailedResponse()
    {
        // Arrange
        var doctorDto = new DoctorCreateRequest
        {
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@example.com",
            Password = "Password123"
        };

        _mockUserService.Setup(s => s.CreateApplicationUser(
            It.IsAny<ApplicationUser>(),
            doctorDto.Password,
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(ServiceResponse.Failed("User creation failed"));

        // Act
        var result = await _sut.AddDoctorAsync(doctorDto);

        // Assert
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.ErrorMessage.ShouldBe("User creation failed");
    }

    [Fact]
    public async Task AddDoctorAsync_AddToRoleFails_ReturnsFailedResponse()
    {
        // Arrange
        var doctorDto = new DoctorCreateRequest
        {
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@example.com",
            Password = "Password123"
        };

        _mockUserService.Setup(s => s.CreateApplicationUser(
            It.IsAny<ApplicationUser>(),
            doctorDto.Password,
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(ServiceResponse.Successful())
            ;
        _mockUserService.Setup(s => s.AddUserToRoleAsync(It.IsAny<ApplicationUser>(), UserRole.Doctor, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ServiceResponse.Failed("Role assignment failed"));

        // Act
        var result = await _sut.AddDoctorAsync(doctorDto);

        // Assert
        result.ShouldNotBeNull();
        result.Success.ShouldBeFalse();
        result.ErrorMessage.ShouldBe("Role assignment failed");
    }

    [Fact]
    public async Task AddDoctorAsync_ExceptionThrown_RollsBackTransactionAndRethrows()
    {
        // Arrange
        var doctorDto = new DoctorCreateRequest
        {
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@example.com",
            Password = "Password123"
        };

        _mockUserService.Setup(s => s.CreateApplicationUser(
            It.IsAny<ApplicationUser>(),
            doctorDto.Password,
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(ServiceResponse.Successful());

        _mockUserService.Setup(s => s.AddUserToRoleAsync(It.IsAny<ApplicationUser>(), UserRole.Doctor, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ServiceResponse.Successful());

        _mockDoctorRepository.Setup(r => r.AddAsync(It.IsAny<Doctor>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB error"));

        // Act & Assert
        await Should.ThrowAsync<Exception>(() => _sut.AddDoctorAsync(doctorDto));

        _transactionManager.RolledBack.ShouldBeTrue();
        _transactionManager.Committed.ShouldBeFalse();
        _mockDoctorRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    #endregion

    #region UpdateDoctorAsync

    private void SetupDoctorWithUser(Doctor? doctor) =>
        _mockDoctorRepository
            .Setup(r => r.GetByIdWithIncludes(It.IsAny<int>(), It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Doctor, object>>[]>()))
            .ReturnsAsync(doctor);

    [Fact]
    public async Task UpdateDoctorAsync_DoctorNotExists_ReturnsFailedAndDoesNotUpdateUser()
    {
        // Arrange
        var updateDto = new DoctorUpdateRequest { Id = 1 };
        SetupDoctorWithUser(null);

        // Act
        var result = await _sut.UpdateDoctorAsync(updateDto);

        // Assert
        result.Success.ShouldBeFalse();
        result.ErrorMessage.ShouldBe(string.Format(Resource.NotFoundEntity, Resource.Doctor));
        _mockUserService.Verify(s => s.UpdateUserAsync(It.IsAny<ApplicationUser>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateDoctorAsync_UpdatesTheDoctorsOwnUser()
    {
        // Arrange
        var updateDto = new DoctorUpdateRequest { Id = 1, FirstName = "New", LastName = "Name", PhoneNumber = "555" };
        var ownUser = new ApplicationUser { Id = "own-user" };
        SetupDoctorWithUser(new Doctor { Id = 1, UserId = ownUser.Id, User = ownUser });
        _mockUserService.Setup(s => s.UpdateUserAsync(It.IsAny<ApplicationUser>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ServiceResponse.Successful());

        // Act
        var result = await _sut.UpdateDoctorAsync(updateDto);

        // Assert
        result.Success.ShouldBeTrue();
        _mockUserService.Verify(s => s.UpdateUserAsync(ownUser, It.IsAny<CancellationToken>()), Times.Once);
        _mockUserService.Verify(s => s.GetUserByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        ownUser.FirstName.ShouldBe("New");
        ownUser.PhoneNumber.ShouldBe("555");
    }

    [Fact]
    public async Task UpdateDoctorAsync_UpdateUserFails_ReturnsFailed()
    {
        // Arrange
        var updateDto = new DoctorUpdateRequest { Id = 1 };
        var user = new ApplicationUser { Id = "user-1" };
        SetupDoctorWithUser(new Doctor { Id = 1, User = user });
        _mockUserService.Setup(s => s.UpdateUserAsync(user, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ServiceResponse.Failed("Update failed"));

        // Act
        var result = await _sut.UpdateDoctorAsync(updateDto);

        // Assert
        result.Success.ShouldBeFalse();
        result.ErrorMessage.ShouldBe("Update failed");
        _transactionManager.RolledBack.ShouldBeTrue();
        _mockDoctorRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateDoctorAsync_SpecialtyNotExists_ReturnsFailed()
    {
        // Arrange
        var updateDto = new DoctorUpdateRequest { Id = 1, SpecialtyId = 2 };
        SetupDoctorWithUser(new Doctor { Id = 1, User = new ApplicationUser() });
        _mockSpecialtyRepository.Setup(r => r.ExistsAsync(2, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        var result = await _sut.UpdateDoctorAsync(updateDto);

        // Assert
        result.Success.ShouldBeFalse();
        result.ErrorMessage.ShouldBe(string.Format(Resource.NotFoundEntity, Resource.Specialty));
    }

    [Fact]
    public async Task UpdateDoctorAsync_Successful_ReturnsSuccessful()
    {
        // Arrange
        var updateDto = new DoctorUpdateRequest { Id = 1, SpecialtyId = 2 };
        var user = new ApplicationUser { Id = "user-1" };
        SetupDoctorWithUser(new Doctor { Id = 1, User = user });
        _mockUserService.Setup(s => s.UpdateUserAsync(user, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ServiceResponse.Successful());
        _mockSpecialtyRepository.Setup(r => r.ExistsAsync(2, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var result = await _sut.UpdateDoctorAsync(updateDto);

        // Assert
        result.Success.ShouldBeTrue();
        _transactionManager.Committed.ShouldBeTrue();
        _mockDoctorRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region DeleteDoctorAsync

    [Fact]
    public async Task DeleteDoctorAsync_NoHistory_DeletesAndSaves()
    {
        // Arrange
        _mockDoctorRepository.Setup(r => r.DeleteDoctorAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var result = await _sut.DeleteDoctorAsync(1);

        // Assert
        result.Success.ShouldBeTrue();
        _mockDoctorRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteDoctorAsync_WithClinicalHistory_ReturnsFailedAndDoesNotDelete()
    {
        // Arrange
        _mockDoctorRepository.Setup(r => r.HasClinicalHistoryAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var result = await _sut.DeleteDoctorAsync(1);

        // Assert
        result.Success.ShouldBeFalse();
        result.ErrorMessage.ShouldBe(string.Format(Resource.CannotDeleteEntityWithRelationships, Resource.Doctor, Resource.ClinicalHistory));
        _mockDoctorRepository.Verify(r => r.DeleteDoctorAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        _mockDoctorRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DeleteDoctorAsync_NotFound_ReturnsFailed()
    {
        // Arrange
        _mockDoctorRepository.Setup(r => r.DeleteDoctorAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        var result = await _sut.DeleteDoctorAsync(1);

        // Assert
        result.Success.ShouldBeFalse();
        result.ErrorMessage.ShouldBe(string.Format(Resource.NotFoundEntity, Resource.Doctor));
        _mockDoctorRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    #endregion

    #region GetDoctorProfileByUserIdAsync

    [Fact]
    public async Task GetDoctorProfileByUserIdAsync_DoctorExists_ReturnsProfileResponse()
    {
        // Arrange
        var userId = "user-1";
        var doctor = new Doctor
        {
            Id = 1,
            UserId = userId,
            FirstName = "John",
            LastName = "Doe",
            Specialty = new Specialty { Name = "Cardiology" },
            User = new ApplicationUser { Email = "john.doe@example.com", PhoneNumber = "1234567890" }
        };
        _mockDoctorRepository.Setup(r => r.GetDoctorByUserIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(doctor);

        // Act
        var result = await _sut.GetDoctorProfileByUserIdAsync(userId);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<ProfileResponse>();
        result.Id.ShouldBe(doctor.Id);
        result.Email.ShouldBe(doctor.User.Email);
    }

    [Fact]
    public async Task GetDoctorProfileByUserIdAsync_DoctorNotFound_ReturnsNull()
    {
        // Arrange
        var userId = "user-1";
        _mockDoctorRepository.Setup(r => r.GetDoctorByUserIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync((Doctor?)null);

        // Act
        var result = await _sut.GetDoctorProfileByUserIdAsync(userId);

        // Assert
        result.ShouldBeNull();
    }

    #endregion

    #region GetMyPatientsAsync

    [Fact]
    public async Task GetMyPatientsAsync_DoctorNotFound_ReturnsEmpty()
    {
        // Arrange
        var userId = "user-1";
        _mockDoctorRepository.Setup(r => r.GetDoctorByUserIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync((Doctor?)null);

        // Act
        var result = await _sut.GetMyPatientsAsync(userId);

        // Assert
        result.ShouldBeEmpty();
    }

    #endregion

    #region GetDoctorCountAsync

    [Fact]
    public async Task GetDoctorCountAsync_ReturnsCount()
    {
        // Arrange
        _mockDoctorRepository.Setup(r => r.GetCountAsync(It.IsAny<CancellationToken>())).ReturnsAsync(5);

        // Act
        var result = await _sut.GetDoctorCountAsync();

        // Assert
        result.ShouldBe(5);
    }

    #endregion

    #region GetAllDoctorsSimpleAsync

    [Fact]
    public async Task GetAllDoctorsSimpleAsync_ReturnsSimpleDtos()
    {
        // Arrange
        var doctors = new List<Doctor>
        {
            new Doctor { Id = 1, Image = "img1" },
            new Doctor { Id = 2, Image = "img2" }
        };
        _mockDoctorRepository.Setup(r => r.GetAllDoctorsSimpleAsync(It.IsAny<CancellationToken>())).ReturnsAsync(doctors);

        // Act
        var result = await _sut.GetAllDoctorsSimpleAsync();

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeAssignableTo<IEnumerable<DoctorSimpleResponse>>();
    }

    #endregion
}
