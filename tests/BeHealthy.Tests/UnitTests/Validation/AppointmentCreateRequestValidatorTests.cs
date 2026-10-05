using BeHealthy.Shared.Common;
using BeHealthy.Shared.Dtos.Appointment;
using BeHealthy.Tests.UnitTests.Fakes;
using BeHealthy.Validation.Appointments;
using FluentValidation.TestHelper;

namespace BeHealthy.Tests.UnitTests.Validation;

public class AppointmentCreateRequestValidatorTests
{
    private static AppointmentCreateRequest ValidRequest() => new()
    {
        PatientId = 1,
        DoctorId = 2,
        AppointmentDate = new DateOnly(2026, 10, 10),
        AppointmentStartTime = new TimeOnly(9, 0),
        AppointmentEndTime = new TimeOnly(9, 30),
        Reason = AppointmentReason.GeneralCheckup,
        Status = AppointmentStatus.Scheduled
    };

    private static AppointmentCreateRequestValidator CreateValidator(params string[] enabledSettings)
        => new(new FakeValidationSettingsProvider(enabledSettings));

    [Fact]
    public async Task Validate_ValidRequest_HasNoErrors()
    {
        // Act
        var result = await CreateValidator().TestValidateAsync(ValidRequest());

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(9, 0)]
    [InlineData(8, 0)]
    public async Task Validate_EndTimeNotAfterStartTime_HasError(int endHour, int endMinute)
    {
        // Arrange
        var request = ValidRequest();
        request.AppointmentEndTime = new TimeOnly(endHour, endMinute);

        // Act
        var result = await CreateValidator().TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.AppointmentEndTime)
            .WithErrorMessage(Resource.EndTimeMustBeLaterThanStartTime);
    }

    [Fact]
    public async Task Validate_EnumValueOutsideTheDefinedRange_HasError()
    {
        // Arrange: JsonStringEnumConverter accepts any integer, so 99 can reach the API.
        var request = ValidRequest();
        request.Reason = (AppointmentReason)99;

        // Act
        var result = await CreateValidator().TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Reason);
    }

    [Fact]
    public async Task Validate_MissingDoctorAndPatient_HasErrors()
    {
        // Arrange
        var request = ValidRequest();
        request.DoctorId = 0;
        request.PatientId = 0;

        // Act
        var result = await CreateValidator().TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DoctorId);
        result.ShouldHaveValidationErrorFor(x => x.PatientId);
    }

    [Fact]
    public async Task Validate_NoRoomAndRoomSettingOn_HasError()
    {
        // Arrange
        var validator = CreateValidator(AppSettingKeys.AppointmentRequiresRoom);

        // Act
        var result = await validator.TestValidateAsync(ValidRequest());

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RoomId);
        result.ShouldNotHaveValidationErrorFor(x => x.NurseId);
    }

    [Fact]
    public async Task Validate_NoNurseAndNurseSettingOn_HasError()
    {
        // Arrange
        var validator = CreateValidator(AppSettingKeys.NurseIsRequiredForAppointment);

        // Act
        var result = await validator.TestValidateAsync(ValidRequest());

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.NurseId);
        result.ShouldNotHaveValidationErrorFor(x => x.RoomId);
    }

    [Fact]
    public async Task Validate_NotesLongerThanTheColumn_HasError()
    {
        // Arrange
        var request = ValidRequest();
        request.Notes = new string('n', 501);

        // Act
        var result = await CreateValidator().TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Notes);
    }
}
