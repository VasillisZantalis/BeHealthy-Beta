using BeHealthy.Application.Validators.Appointments;
using BeHealthy.Application.Validators.Doctors;
using BeHealthy.Application.Validators.Settings;
using BeHealthy.Shared.Dtos.Appointment;
using FluentValidation.TestHelper;

namespace BeHealthy.Tests.UnitTests.Validation;

public class ServerValidatorTests
{
    #region DoctorCreateRequestServerValidator

    [Fact]
    public async Task DoctorCreate_EmailAlreadyInUse_HasError()
    {
        // Arrange
        var userService = new Mock<IUserService>();
        userService.Setup(s => s.IsEmailInUseAsync("taken@ppth.org", It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var sut = new DoctorCreateRequestServerValidator(userService.Object, Mock.Of<IDepartmentRepository>(), Mock.Of<ISpecialtyRepository>());

        // Act
        var result = await sut.TestValidateAsync(new DoctorCreateRequest { Email = "taken@ppth.org" });

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage(Resource.EmailAlreadyUsed);
    }

    [Fact]
    public async Task DoctorCreate_UnknownDepartmentAndSpecialty_HasErrors()
    {
        // Arrange
        var departments = new Mock<IDepartmentRepository>();
        departments.Setup(r => r.ExistsAsync(7, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        var specialties = new Mock<ISpecialtyRepository>();
        specialties.Setup(r => r.ExistsAsync(8, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        var sut = new DoctorCreateRequestServerValidator(Mock.Of<IUserService>(), departments.Object, specialties.Object);

        // Act
        var result = await sut.TestValidateAsync(new DoctorCreateRequest { Email = "new@ppth.org", DepartmentId = 7, SpecialtyId = 8 });

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DepartmentId).WithErrorMessage(string.Format(Resource.NotFoundEntity, Resource.Department));
        result.ShouldHaveValidationErrorFor(x => x.SpecialtyId).WithErrorMessage(string.Format(Resource.NotFoundEntity, Resource.Specialty));
    }

    [Fact]
    public async Task DoctorCreate_NoOptionalReferencesOrEmail_DoesNotQueryTheDatabase()
    {
        // Arrange: empty/absent values are the shared validator's job, so no lookups and no second message.
        var userService = new Mock<IUserService>(MockBehavior.Strict);
        var departments = new Mock<IDepartmentRepository>(MockBehavior.Strict);
        var specialties = new Mock<ISpecialtyRepository>(MockBehavior.Strict);
        var sut = new DoctorCreateRequestServerValidator(userService.Object, departments.Object, specialties.Object);

        // Act
        var result = await sut.TestValidateAsync(new DoctorCreateRequest { Email = "", DepartmentId = null, SpecialtyId = 0 });

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    #endregion

    #region AppointmentCreateRequestServerValidator

    [Fact]
    public async Task AppointmentCreate_PatientDoesNotExist_HasError()
    {
        // Arrange
        var patients = new Mock<IPatientRepository>();
        patients.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        var doctors = new Mock<IDoctorRepository>();
        doctors.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var sut = new AppointmentCreateRequestServerValidator(patients.Object, doctors.Object, Mock.Of<IRoomRepository>(), Mock.Of<INurseRepository>());

        // Act
        var result = await sut.TestValidateAsync(new AppointmentCreateRequest { PatientId = 42, DoctorId = 1 });

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PatientId).WithErrorMessage(string.Format(Resource.NotFoundEntity, Resource.Patient));
        result.ShouldNotHaveValidationErrorFor(x => x.DoctorId);
    }

    #endregion

    #region AppSettingUpdateRequestServerValidator

    [Theory]
    [InlineData(SettingType.Checkbox, "maybe", true)]
    [InlineData(SettingType.Checkbox, "true", false)]
    [InlineData(SettingType.SingleSelect, "two", true)]
    [InlineData(SettingType.SingleSelect, "2", false)]
    public async Task AppSettingUpdate_ValueMustMatchTheSettingType(SettingType type, string value, bool expectError)
    {
        // Arrange
        var repository = new Mock<IAppSettingsRepository>();
        repository.Setup(r => r.GetSettingByKeyAsync("Key", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AppSetting { Key = "Key", Type = type, Caption = "Caption" });
        var sut = new AppSettingUpdateRequestServerValidator(repository.Object);

        // Act
        var result = await sut.TestValidateAsync(new AppSettingUpdateRequest { Key = "Key", Value = value });

        // Assert
        if (expectError)
        {
            result.ShouldHaveValidationErrorFor(x => x.Value);
        }
        else
        {
            result.ShouldNotHaveAnyValidationErrors();
        }
    }

    [Fact]
    public async Task AppSettingUpdate_UnknownKey_LeavesItToTheController()
    {
        // Arrange
        var sut = new AppSettingUpdateRequestServerValidator(Mock.Of<IAppSettingsRepository>());

        // Act
        var result = await sut.TestValidateAsync(new AppSettingUpdateRequest { Key = "Missing", Value = "x" });

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    #endregion
}
